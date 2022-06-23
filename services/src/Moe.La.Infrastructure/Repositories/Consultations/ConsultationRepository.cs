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
    public class ConsultationRepository : RepositoryBase, IConsultationRepository
    {
        public ConsultationRepository(LaDbContext commandDb, IMapper mapperConfig, IUserProvider userProvider)
                : base(commandDb, mapperConfig, userProvider)
        {
        }

        public async Task<QueryResultDto<ConsultationListItemDto>> GetAllAsync(ConsultationQueryObject queryObject)
        {

            var result = new QueryResult<ConsultationMoamalat>();

            var query = db.ConsultationMoamalat
                .Include(c => c.Consultation)
                .ThenInclude(c => c.CreatedByUser)
                .Include(c => c.Moamala)
                .ThenInclude(c => c.ReceiverDepartment)
                .Include(c => c.Moamala)
                .ThenInclude(c => c.WorkItemType)
                .Include(c => c.Moamala)
                .ThenInclude(c => c.AssignedTo)
                .Include(c => c.Consultation)
                .ThenInclude(c => c.ConsultationTransactions)
                .Where(c => c.CreatedOn.Date == c.Consultation.CreatedOn.Date /*&& c.Consultation.CreatedBy==UserId*/)
                .AsQueryable();


            // get userRoleDepartments
            var UserRoleDepartments = await db.AppUserRoleDepartmets
                .Where(d => d.UserId == CurrentUser.UserId).Select(r => r.DepartmentId).ToListAsync();

            // researcher can see his models
            if (CurrentUser.Roles.Contains(ApplicationRolesConstants.LegalResearcher.Name))
            {
                query = query.Where(x => x.Consultation.CreatedBy == CurrentUser.UserId && (x.Consultation.Status == ConsultationStatus.Returned || x.Consultation.Status == ConsultationStatus.Draft));
            }

            // Department manager can see his employee models except confidential ones & returned specialy to him 
            if (CurrentUser.Roles.Contains(ApplicationRolesConstants.DepartmentManager.Name))
            {

                query = query.Where(x => x.Moamala.ConfidentialDegree != ConfidentialDegrees.Confidential
                && UserRoleDepartments.Contains(x.Moamala.ReceiverDepartmentId.Value)
                && (x.Consultation.Status == ConsultationStatus.New ||
                (x.Consultation.Status == ConsultationStatus.Modified && x.Consultation.ReturnedType == ReturnedConsultationTypes.FromDepartmentManager)));
            }
            // General supervisor can see all models even if ones with confidential degree & returned  specialy to him 

            if (CurrentUser.Roles.Contains(ApplicationRolesConstants.GeneralSupervisor.Name))
            {
                query = query.Where(x => x.Consultation.Status == ConsultationStatus.Accepted ||
                (x.Consultation.Status == ConsultationStatus.Modified && x.Consultation.ReturnedType == ReturnedConsultationTypes.FromGeneralSupervisor)
                || (x.Consultation.Status == ConsultationStatus.New && x.Moamala.ConfidentialDegree == ConfidentialDegrees.Confidential));
            }
            // Distributor can see models except if he has confidential degree
            if (CurrentUser.Roles.Contains(ApplicationRolesConstants.Distributor.Name))
            {
                if (queryObject.HasConfidentialAccess)
                {
                    query = query.Where(x => x.Consultation.Status == ConsultationStatus.Approved);
                }
                else
                {
                    query = query.Where(x => x.Consultation.Status == ConsultationStatus.Approved && x.Moamala.ConfidentialDegree != ConfidentialDegrees.Confidential);
                }
            }

            if (queryObject.DepartmentId.HasValue)
                query = query.Where(s => s.Moamala.ReceiverDepartmentId == queryObject.DepartmentId);
            if (queryObject.WorkItemTypeId.HasValue)
                query = query.Where(s => s.Moamala.WorkItemTypeId == queryObject.WorkItemTypeId);
            if (queryObject.Status.HasValue)
                query = query.Where(s => s.Consultation.Status == queryObject.Status);

            if (queryObject.DateFrom.HasValue)
            {
                query = query.Where(p => p.Moamala.CreatedOn.Date >= queryObject.DateFrom.Value.Date);
            }

            if (queryObject.DateTo.HasValue)
            {
                query = query.Where(p => p.Moamala.CreatedOn.Date <= queryObject.DateTo.Value.Date);
            }

            if (queryObject.AssignedTo.HasValue)
            {
                query = query.Where(p => p.Moamala.AssignedToId == queryObject.AssignedTo.Value);
            }

            if (!string.IsNullOrEmpty(queryObject.SearchText))
            {
                query = query.Where(m =>
                m.Consultation.Subject.Contains(queryObject.SearchText) ||
                m.Consultation.LegalAnalysis.Contains(queryObject.SearchText) ||
                m.Moamala.Subject.Contains(queryObject.SearchText)
                );
            }
            var columnsMap = new Dictionary<string, Expression<Func<ConsultationMoamalat, object>>>()
            {
                ["moamalaNumber"] = v => v.Moamala.MoamalaNumber,
                ["subject"] = v => v.Moamala.Subject,
                ["moamalaDate"] = v => v.Moamala.CreatedOn,
                ["workItemType"] = v => v.Moamala.WorkItemType.Name,
                ["department"] = v => v.Moamala.ReceiverDepartment.Name,
            };

            query = query.ApplySorting(queryObject, columnsMap);

            result.TotalItems = await query.CountAsync();

            query = query.ApplyPaging(queryObject);

            result.Items = await query.AsNoTracking().ToListAsync();

            return mapper.Map<QueryResult<ConsultationMoamalat>, QueryResultDto<ConsultationListItemDto>>(result);
        }

        public async Task<ConsultationDetailsDto> GetAsync(int id)
        {
            var consultation = await db.Consultations
                   .Include(g => g.ConsultationGrounds)
                   .Include(g => g.ConsultationMerits)
                   .Include(g => g.ConsultationVisuals)
                   .Include(m => m.ConsultationMoamalat)
                   .Include(d => d.Department)
                   .Include(c => c.Branch)
                   .Include(c => c.WorkItemType)
                   .Include(c => c.SubWorkItemType)
                   .Include(c => c.ConsultationTransactions)
                   .ThenInclude(u => u.CreatedByUser)
                    .ThenInclude(r => r.UserRoles)
                        .ThenInclude(r => r.Role)
                   .Include(c => c.ConsultationSupportingDocuments)
                       .ThenInclude(cc => cc.Request)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);

            return mapper.Map<Consultation, ConsultationDetailsDto>(consultation);
        }

        public async Task AddAsync(ConsultationDto consultationDto)
        {
            var entityToAdd = mapper.Map<Consultation>(consultationDto);

            entityToAdd.CreatedBy = CurrentUser.UserId;
            entityToAdd.CreatedOn = DateTime.Now;

            await db.Consultations.AddAsync(entityToAdd);
            await db.SaveChangesAsync();

            // get consultation id
            consultationDto.Id = entityToAdd.Id;

            // insert in consultation moamalat
            db.ConsultationMoamalat.Add(new ConsultationMoamalat
            {
                ConsultationId = entityToAdd.Id,
                MoamalaId = consultationDto.MoamalaId,
                CreatedOn = DateTime.Now
            });
            await db.SaveChangesAsync();
        }
        public async Task EditAsync(ConsultationDto consultationDto)
        {
            var entityToUpdate = await db.Consultations
                .Include(g => g.ConsultationGrounds)
                .Include(g => g.ConsultationMerits)
                .Include(g => g.ConsultationVisuals)
                .FirstOrDefaultAsync(s => s.Id == consultationDto.Id);

            db.ConsultationGrounds.RemoveRange(entityToUpdate.ConsultationGrounds);
            db.ConsultationMerits.RemoveRange(entityToUpdate.ConsultationMerits);
            db.ConsultationVisuals.RemoveRange(entityToUpdate.ConsultationVisuals);
            await db.SaveChangesAsync();
            mapper.Map(consultationDto, entityToUpdate);

            entityToUpdate.UpdatedBy = CurrentUser.UserId;
            entityToUpdate.UpdatedOn = DateTime.Now;

            if (entityToUpdate.Status == ConsultationStatus.Returned)
                entityToUpdate.Status = ConsultationStatus.Modified;

            await db.SaveChangesAsync();

            mapper.Map(entityToUpdate, consultationDto);

        }

        public async Task DeleteVisualAsync(int visualId)
        {
            var entityToDelete = await db.ConsultationVisuals.FindAsync(visualId);
            db.ConsultationVisuals.Remove(entityToDelete);
            await db.SaveChangesAsync();
        }
        public async Task<ConsultationReviewDto> ConsultationReviewAsync(ConsultationReviewDto ConsultationReviewDto)
        {
            var entityToUpdate = await db.Consultations
                  .Where(m => m.Id == ConsultationReviewDto.Id)
                  .FirstOrDefaultAsync();

            entityToUpdate.DepartmentVision = ConsultationReviewDto.DepartmentVision;
            entityToUpdate.ReturnedType = ConsultationReviewDto.ReturnedType;
            entityToUpdate.UpdatedOn = DateTime.Now;

            entityToUpdate.Status = ConsultationReviewDto.ConsultationStatus;

            await db.SaveChangesAsync();

            return mapper.Map<ConsultationReviewDto>(ConsultationReviewDto);
        }
    }
}