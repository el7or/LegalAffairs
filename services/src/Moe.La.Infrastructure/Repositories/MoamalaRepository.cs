using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moe.La.Core.Constants;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Enums;
using Moe.La.Core.Interfaces.Repositories;
using Moe.La.Core.Interfaces.Services;
using Moe.La.Infrastructure.DbContexts;
using Moe.La.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Moe.La.Infrastructure.Repositories
{

    public class MoamalaRepository : RepositoryBase, IMoamalaRepository
    {

        public MoamalaRepository(LaDbContext context, IMapper mapperConfig, IUserProvider userProvider)
            : base(context, mapperConfig, userProvider)
        {
        }

        public async Task<QueryResultDto<MoamalaListItemDto>> GetAllAsync(MoamalatQueryObject queryObject)
        {
            var userRoleManagedDepartments = await db.AppUserRoleDepartmets
                .Where(d => d.UserId == CurrentUser.UserId && d.RoleId == ApplicationRolesConstants.DepartmentManager.Code)
                .Select(r => r.DepartmentId).ToListAsync();

            CurrentUser.BranchId = db.Users.Find(CurrentUser.UserId).BranchId.Value;

            var result = new QueryResult<Moamala>();

            var query = db.Moamalat
                .Where(d => d.IsDeleted == false)
                .Include(d => d.SenderDepartment)
                .Include(d => d.WorkItemType)
                .Include(d => d.SubWorkItemType)
                .Include(d => d.Branch)
                .Include(d => d.ReceiverDepartment)
                .Include(d => d.MoamalaNotifications)
                .Include(d => d.CreatedByUser)
                .Include(d => d.ConsultationMoamalat)
                .AsQueryable();


            query = query.Where(p =>

                (p.CurrentStep == MoamalaSteps.Initial &&

                // return all for general supervisor
                (CurrentUser.Roles.Contains(ApplicationRolesConstants.GeneralSupervisor.Name) ||

                // return all for general Distributor with confidential permission
                (CurrentUser.Roles.Contains(ApplicationRolesConstants.Distributor.Name) && CurrentUser.Permissions.Contains(ApplicationRoleClaimsConstants.ConfidentialMoamla.ClaimValue) && p.ConfidentialDegree == ConfidentialDegrees.Confidential) ||

                // return all for general Distributor except confidential moamala
                (CurrentUser.Roles.Contains(ApplicationRolesConstants.Distributor.Name) && p.ConfidentialDegree != ConfidentialDegrees.Confidential))) ||

                //////////////////////
                // return user assigned moalalat
                (p.CurrentStep == MoamalaSteps.Employee && p.AssignedToId == CurrentUser.UserId) ||

                // return moamalat to BranchManager refered to his generaldepartment
                (p.CurrentStep == MoamalaSteps.Branch && CurrentUser.Roles.Contains(ApplicationRolesConstants.BranchManager.Name) && p.BranchId == CurrentUser.BranchId) ||

                // return moamalat to DepartmentManager refered to his department
                (p.CurrentStep == MoamalaSteps.Department && CurrentUser.Roles.Contains(ApplicationRolesConstants.DepartmentManager.Name) && p.ReceiverDepartmentId.HasValue && userRoleManagedDepartments.Contains((int)p.ReceiverDepartmentId))
                );


            if (queryObject.AssignedToId.HasValue)
            {
                query = query.Where(p => p.AssignedToId == queryObject.AssignedToId);
            }

            if (queryObject.SenderDepartmentId.HasValue)
            {
                query = query.Where(p => p.SenderDepartmentId == queryObject.SenderDepartmentId);
            }

            if (queryObject.ReceiverDepartmentId.HasValue)
            {
                query = query.Where(p => p.ReceiverDepartmentId == queryObject.ReceiverDepartmentId);
            }
            if (queryObject.RelatedId.HasValue)
            {
                query = query.Where(p => p.RelatedId == queryObject.RelatedId);
            }

            if (queryObject.Status.HasValue)
            {
                if (queryObject.Status != 0) // we set all value to 0
                {
                    query = query.Where(s => (int)s.Status == queryObject.Status);
                }
            }
            if (queryObject.ConfidentialDegree.HasValue)
            {
                if (queryObject.ConfidentialDegree != 0) // we set all value to 0
                {
                    query = query.Where(s => (int)s.ConfidentialDegree == queryObject.ConfidentialDegree);
                }
            }

            if (queryObject.CreatedOnFrom.HasValue)
            {
                query = query.Where(p => p.CreatedOn.Date >= queryObject.CreatedOnFrom.Value.Date);
            }

            if (queryObject.CreatedOnTo.HasValue)
            {
                query = query.Where(p => p.CreatedOn.Date <= queryObject.CreatedOnTo.Value.Date);
            }


            if (!string.IsNullOrEmpty(queryObject.SearchText))
            {
                query = query.Where(m =>
                    m.Subject.Contains(queryObject.SearchText) ||
                    m.Description.Contains(queryObject.SearchText)
                );
            }


            var columnsMap = new Dictionary<string, Expression<Func<Moamala, object>>>()
            {
                ["unifiedNo"] = v => v.UnifiedNo,
                ["moamalaNumber"] = v => v.MoamalaNumber,
                ["createdOn"] = v => v.CreatedOn,
                ["passDate"] = v => v.PassDate,
                ["subject"] = v => v.Subject,
                ["description"] = v => v.Description,
                ["confidentialDegree"] = v => v.ConfidentialDegree,
                ["status"] = v => v.Status,
                ["department"] = v => v.SenderDepartment.Name,
            };

            query = query.ApplySorting(queryObject, columnsMap);
            result.TotalItems = await query.CountAsync();

            query = query.ApplyPaging(queryObject);

            result.Items = await query.AsNoTracking().ToListAsync();

            // get current user moamala status >> isread
            foreach (var moamala in result.Items)
            {
                moamala.MoamalaNotifications = moamala.MoamalaNotifications.Where(n => n.UserId == CurrentUser.UserId).ToList();
            }

            return mapper.Map<QueryResult<Moamala>, QueryResultDto<MoamalaListItemDto>>(result);
        }

        public async Task<MoamalaDetailsDto> GetAsync(int? id)
        {
            var moamala = await db.Moamalat
                .Where(d => d.IsDeleted == false)
                .Include(d => d.SenderDepartment)
                .Include(d => d.Branch)
                .Include(d => d.ReceiverDepartment)
                .Include(d => d.WorkItemType)
                .Include(d => d.SubWorkItemType)
                .Include(m => m.AssignedTo)
                //.Include(m => m.CreatedByUser)
                .Include(m => m.ConsultationMoamalat)
                    .ThenInclude(c => c.Consultation)
                .Include(m => m.Attachments)
                  .ThenInclude(m => m.Attachment)
                .Include(m => m.Attachments)
                  .ThenInclude(m => m.Attachment)
                  .ThenInclude(m => m.AttachmentType)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);
            var model = mapper.Map<MoamalaDetailsDto>(moamala);

            var relatedMoamalat = db.Moamalat
                .Where(m => m.UnifiedNo == moamala.UnifiedNo && m.Id != moamala.Id)
                .Include(d => d.SenderDepartment)
                .Include(d => d.WorkItemType)
                .Include(d => d.Branch)
                .Include(d => d.ReceiverDepartment)
                .Include(m => m.CreatedByUser)
                .ToList();

            model.RelatedMoamalat = mapper.Map<List<Moamala>, List<MoamalaListItemDto>>(relatedMoamalat);

            if (model.WorkItemType != null)
            {
                switch (model.WorkItemType.Name)
                {
                    case "قضية":
                        model.ReleatedItems = db.Cases.Select(c => new KeyValuePairsDto<int>
                        {
                            Id = c.Id,
                            Name = "رقم القضية: " + c.Id + " - اسم القضية: " + c.Subject
                        }).ToList();
                        model.ReleatedItemsTitle = model.ReleatedItems.Count > 0 ? "قائمة القضايا" : "لا يوجد أي قضية في النظام";
                        break;

                    case "تحقيق":
                        model.ReleatedItemsTitle = "قائمة التحقيقات";
                        model.ReleatedItems = db.Investigations.Select(i => new KeyValuePairsDto<int>
                        {
                            Id = i.Id,
                            Name = "رقم التحقيق: " + i.Id + " - اسم التحقيق: " + i.Subject
                        }).ToList();
                        model.ReleatedItemsTitle = model.ReleatedItems.Count > 0 ? "قائمة التحقيقات" : "لا يوجد أي تحقيق في النظام";
                        break;

                    case "استشارة":
                        break;

                    case "دراسة قانونية":
                        model.ReleatedItems = db.Consultations.Where(c => c.WorkItemTypeId == 2)
                            .Select(i => new KeyValuePairsDto<int>
                            {
                                Id = i.Id,
                                Name = "رقم الدراسة القانونية: " + i.Id + " - اسم الدراسة القانونية: " + i.Subject
                            }).ToList();
                        model.ReleatedItemsTitle = model.ReleatedItems.Count > 0 ? "قائمة الدراسات القانونية" : "لا يوجد أي دراسة قانونية في النظام";
                        break;

                    case "تظلم":
                        model.ReleatedItems = db.Consultations.Where(c => c.WorkItemTypeId == 3)
                            .Select(i => new KeyValuePairsDto<int>
                            {
                                Id = i.Id,
                                Name = "رقم التظلم: " + i.Id + " - اسم التظلم: " + i.Subject
                            }).ToList();
                        model.ReleatedItemsTitle = model.ReleatedItems.Count > 0 ? "قائمة التظلمات" : "لا يوجد أي تظلم في النظام";
                        break;

                    case "تسوية قانونية":
                        model.ReleatedItems = db.Consultations.Where(c => c.WorkItemTypeId == 4)
                            .Select(i => new KeyValuePairsDto<int>
                            {
                                Id = i.Id,
                                Name = "رقم التسوية القانونية: " + i.Id + " - اسم التسوية القانونية: " + i.Subject
                            }).ToList();
                        model.ReleatedItemsTitle = model.ReleatedItems.Count > 0 ? "قائمة التسويات القانونية" : "لا يوجد أي تسوية قانونية في النظام";
                        break;

                    case "عقد":
                        model.ReleatedItems = db.Consultations.Where(c => c.WorkItemTypeId == 5)
                            .Select(i => new KeyValuePairsDto<int>
                            {
                                Id = i.Id,
                                Name = "رقم العقد: " + i.Id + " - اسم العقد: " + i.Subject
                            }).ToList();
                        model.ReleatedItemsTitle = model.ReleatedItems.Count > 0 ? "قائمة العقود" : "لا يوجد أي عقد في النظام";
                        break;

                    case "اتفاقية قانونية":
                        model.ReleatedItems = db.Consultations.Where(c => c.WorkItemTypeId == 6)
                            .Select(i => new KeyValuePairsDto<int>
                            {
                                Id = i.Id,
                                Name = "رقم الاتفاقية القانونية: " + i.Id + " - اسم الاتفاقية القانونية: " + i.Subject
                            }).ToList();
                        model.ReleatedItemsTitle = model.ReleatedItems.Count > 0 ? "قائمة الاتفاقيات القانونية" : "لا يوجد أي اتفاقية قانونية في النظام";
                        break;

                    case "معاملة حقوق انسان":
                        model.ReleatedItems = db.Consultations.Where(c => c.WorkItemTypeId == 7)
                            .Select(i => new KeyValuePairsDto<int>
                            {
                                Id = i.Id,
                                Name = "رقم معاملة حقوق الانسان: " + i.Id + " - اسم معاملة حقوق الانسان: " + i.Subject
                            }).ToList();
                        model.ReleatedItemsTitle = model.ReleatedItems.Count > 0 ? "قائمة معاملات حقوق انسان" : "لا يوجد أي معاملة حقوق انسان في النظام";
                        break;

                    case "نظام":
                        model.ReleatedItems = db.Consultations.Where(c => c.WorkItemTypeId == 8)
                            .Select(i => new KeyValuePairsDto<int>
                            {
                                Id = i.Id,
                                Name = "رقم النظام: " + i.Id + " - اسم النظام: " + i.Subject
                            }).ToList();
                        model.ReleatedItemsTitle = model.ReleatedItems.Count > 0 ? "قائمة الأنظمة" : "لا يوجد أي نظام في النظام";
                        break;

                    case "لائحة":
                        model.ReleatedItems = db.Consultations.Where(c => c.WorkItemTypeId == 9)
                            .Select(i => new KeyValuePairsDto<int>
                            {
                                Id = i.Id,
                                Name = "رقم اللائحة: " + i.Id + " - اسم اللائحة: " + i.Subject
                            }).ToList();
                        model.ReleatedItemsTitle = model.ReleatedItems.Count > 0 ? "قائمة اللوائح" : "لا يوجد أي لائحة في النظام";
                        break;

                    case "قرار":
                        break;

                    default:
                        break;
                }

            }
            // make moamala notification readed
            var moamalaNotification = db.MoamalatNotifications.FirstOrDefault(m => m.MoamalaId == id && m.UserId == CurrentUser.UserId);
            if (moamalaNotification != null)
            {
                moamalaNotification.IsRead = true;
                await db.SaveChangesAsync();
            }
            else
            {
                await AddNotificationAsync(new MoamalaNotificationDto { MoamalaId = id.Value, UserId = CurrentUser.UserId, IsRead = true });
            }
            return model;
        }

        public async Task AddAsync(MoamalaDto moamlaDto)
        {
            var entityToAdd = mapper.Map<Moamala>(moamlaDto);

            entityToAdd.CreatedBy = CurrentUser.UserId;
            entityToAdd.CreatedOn = DateTime.Now;
            entityToAdd.CurrentStep = MoamalaSteps.Initial;

            await db.Moamalat.AddAsync(entityToAdd);
            await db.SaveChangesAsync();

            ///  notification should be sent to all distriputers && general supervisors
            //add notificatiom with unread value

            var users = await db.Users.Where(u => u.UserRoles.Any(ur => ur.RoleId == ApplicationRolesConstants.GeneralSupervisor.Code || ur.RoleId == ApplicationRolesConstants.Distributor.Code)).ToListAsync();
            if (entityToAdd.ConfidentialDegree == ConfidentialDegrees.Confidential)
            {
                users = users.Where(u => u.UserRoles.Any(ur => ur.RoleId == ApplicationRolesConstants.GeneralSupervisor.Code) || u.UserClaims.Any(c => c.ClaimValue == ApplicationRoleClaimsConstants.ConfidentialMoamla.ClaimValue)).ToList();
            }
            foreach (var user in users)
                await AddNotificationAsync(new MoamalaNotificationDto { MoamalaId = entityToAdd.Id, UserId = user.Id });
            await db.SaveChangesAsync();
            moamlaDto.Id = entityToAdd.Id;
        }

        public async Task EditAsync(MoamalaDto moamlaDto)
        {
            var entityToUpdate = await db.Moamalat
                .Include(m => m.Attachments)
                .Where(m => m.Id == moamlaDto.Id).FirstOrDefaultAsync();

            mapper.Map(moamlaDto, entityToUpdate);

            await db.SaveChangesAsync();
        }

        public async Task RemoveAsync(int id)
        {
            var entityToDelete = await db.Moamalat
                .FindAsync(id);

            entityToDelete.IsDeleted = true;

            await db.SaveChangesAsync();
        }

        public async Task ChangeStatusAsync(MoamalaChangeStatusDto changeStatusDto)
        {
            var entityToUpdate = await db.Moamalat.FindAsync(changeStatusDto.MoamalaId);

            //// 
            entityToUpdate.WorkItemTypeId = changeStatusDto.WorkItemTypeId == null ? entityToUpdate.WorkItemTypeId : changeStatusDto.WorkItemTypeId;
            entityToUpdate.SubWorkItemTypeId = changeStatusDto.subWorkItemTypeId == null ? entityToUpdate.SubWorkItemTypeId : changeStatusDto.subWorkItemTypeId;
            entityToUpdate.Status = changeStatusDto.Status;
            ///

            if (changeStatusDto.Status == MoamalaStatuses.Referred)
            {
                entityToUpdate.PreviousStep = entityToUpdate.CurrentStep;
                entityToUpdate.CurrentStep = changeStatusDto.DepartmentId.HasValue ? MoamalaSteps.Department : MoamalaSteps.Branch;

                entityToUpdate.BranchId = changeStatusDto.BranchId;
                entityToUpdate.ReceiverDepartmentId = changeStatusDto.DepartmentId == null ? entityToUpdate.ReceiverDepartmentId : changeStatusDto.DepartmentId;
                entityToUpdate.ReferralNote = changeStatusDto.Note;

            }

            else if (changeStatusDto.Status == MoamalaStatuses.Assigned)
            {
                entityToUpdate.PreviousStep = entityToUpdate.CurrentStep;
                entityToUpdate.CurrentStep = MoamalaSteps.Employee;
                entityToUpdate.AssignedToId = changeStatusDto.AssignedToId;
                entityToUpdate.AssigningNote = changeStatusDto.Note;
            }

            await db.SaveChangesAsync();
        }

        public async Task ReturnAsync(int id, string note)
        {
            var entityToUpdate = await db.Moamalat.FindAsync(id);

            entityToUpdate.ReturningReason = note;
            entityToUpdate.Status = MoamalaStatuses.MoamalaReturned;

            entityToUpdate.CurrentStep = (MoamalaSteps)entityToUpdate.PreviousStep;

            var newPrevStepValue = (int)entityToUpdate.CurrentStep - 1;

            entityToUpdate.PreviousStep = newPrevStepValue == 0 ? null : (MoamalaSteps)newPrevStepValue;


            await db.SaveChangesAsync();

        }

        public async Task AddNotificationAsync(MoamalaNotificationDto moamalaNotificationDto)
        {
            var entityToEdit = db.MoamalatNotifications.FirstOrDefault(m => m.MoamalaId == moamalaNotificationDto.MoamalaId && m.UserId == moamalaNotificationDto.UserId);
            if (entityToEdit == null)
            {
                db.MoamalatNotifications.Add(new MoamalaNotification
                {
                    MoamalaId = moamalaNotificationDto.MoamalaId,
                    UserId = moamalaNotificationDto.UserId,
                    IsRead = moamalaNotificationDto.IsRead,
                    CreatedOn = DateTime.Now
                });
            }
            else
            {
                entityToEdit.IsRead = moamalaNotificationDto.IsRead;
            }
            await db.SaveChangesAsync();
        }

        public async Task<MoamalaDetailsDto> UpdateWorkItemTypeAsync(MoamalaUpdateWorkItemType moamalaUpdateWorkItemType)
        {
            var entityToUpdate = await db.Moamalat.FindAsync(moamalaUpdateWorkItemType.Id);

            entityToUpdate.WorkItemTypeId = moamalaUpdateWorkItemType.WorkItemTypeId;
            entityToUpdate.SubWorkItemTypeId = moamalaUpdateWorkItemType.SubWorkItemTypeId;
            await db.SaveChangesAsync();

            return await GetAsync(entityToUpdate.Id);
        }

        public async Task<MoamalaDetailsDto> UpdateRelatedIdAsync(MoamalaUpdateRelatedId moamalaUpdateRelatedId)
        {
            var entityToUpdate = await db.Moamalat.FindAsync(moamalaUpdateRelatedId.Id);

            entityToUpdate.RelatedId = moamalaUpdateRelatedId.RelatedId;
            await db.SaveChangesAsync();

            return await GetAsync(entityToUpdate.Id);
        }
    }
}
