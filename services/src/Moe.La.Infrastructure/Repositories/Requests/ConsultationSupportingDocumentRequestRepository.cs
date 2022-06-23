using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moe.La.Core.Constants;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Enums;
using Moe.La.Core.Interfaces.Repositories;
using Moe.La.Core.Interfaces.Services;
using Moe.La.Infrastructure.DbContexts;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Moe.La.Infrastructure.Repositories
{
    public class ConsultationSupportingDocumentRepository : RepositoryBase, IConsultationSupportingDocumentRequestRepository
    {

        public ConsultationSupportingDocumentRepository(LaDbContext commandDb, IMapper mapperConfig, IUserProvider userProvider)
            : base(commandDb, mapperConfig, userProvider)
        {
        }

        public async Task<ConsultationSupportingDocumentListItemDto> GetAsync(int id)
        {
            var entity = await db.ConsultationSupportingDocuments
                .Include(m => m.Request)
                  .ThenInclude(m => m.RequestTransactions)
                    .ThenInclude(mm => mm.CreatedByUser)
                     .ThenInclude(ur => ur.UserRoles)
                      .ThenInclude(r => r.Role)
                .Include(m => m.Request)
                  .ThenInclude(mm => mm.CreatedByUser)
                .Where(m => m.RequestId == id).FirstOrDefaultAsync();


            return mapper.Map<ConsultationSupportingDocumentListItemDto>(entity);
        }

        public async Task<ConsultationSupportingDocumentRequestDto> AddAsync(ConsultationSupportingDocumentRequestDto ConsultationSupportingDocumentDto)
        {
            var entityToAdd = mapper.Map<ConsultationSupportingDocumentRequest>(ConsultationSupportingDocumentDto);

            entityToAdd.Request.CreatedBy = CurrentUser.UserId;
            entityToAdd.Request.CreatedOn = DateTime.Now;

            // get the user who assigned the consultation
            var moamalaEntity = await db.Moamalat.FirstOrDefaultAsync(m => m.Id == ConsultationSupportingDocumentDto.MoamalaId);


            if (moamalaEntity.ConfidentialDegree == ConfidentialDegrees.Normal)
            {
                // normal moamalat sent to department
                entityToAdd.Request.SendingType = SendingTypes.Role;
                entityToAdd.Request.ReceiverRoleId = ApplicationRolesConstants.DepartmentManager.Code;
                entityToAdd.Request.ReceiverDepartmentId = moamalaEntity.ReceiverDepartmentId;
                entityToAdd.Request.ReceiverBranchId = moamalaEntity.BranchId;
            }
            else
            {
                // get the user who assigned the consultation
                //var moamalaTransactions = await commandDb.MoamalaTransactions.Include(m => m.CreatedByUser).ThenInclude(u => u.UserRoles).Where(m => m.Id == ConsultationSupportingDocumentDto.MoamalaId).Select(t => t.CreatedByUser).ToListAsync();
                //var moamalaAssignedBy = moamalaTransactions.LastOrDefault();

                //if (moamalaAssignedBy.UserRoles.Any(ur => ur.RoleId == ApplicationRolesConstants.GeneralSupervisor.Code))
                //{
                // confidential moamalatsent to general supervisor to be approved
                entityToAdd.Request.SendingType = SendingTypes.Role;
                entityToAdd.Request.ReceiverRoleId = ApplicationRolesConstants.GeneralSupervisor.Code;
                //}
                //else
                //{
                //    entityToAdd.Request.SendingType = SendingTypes.User;
                //    entityToAdd.Request.ReceiverId = moamalaAssignedBy.Id;
                //}
                //entityToAdd.Request.RequestStatus = RequestStatuses.Accepted;

            }

            entityToAdd.Request.IsExportable = true;
            entityToAdd.CreatedOn = DateTime.Now;

            await db.ConsultationSupportingDocuments.AddAsync(entityToAdd);
            await db.SaveChangesAsync();

            return mapper.Map(entityToAdd, ConsultationSupportingDocumentDto);
        }

        public async Task<ConsultationSupportingDocumentRequestDto> ReplyConsultationSupportingDocumentAsync(ReplyConsultationSupportingDocumentDto replyConsultationSupportingDocumentDto)
        {
            var entityToUpdate = await db.ConsultationSupportingDocuments
                  .Include(m => m.Request)
                  .Where(m => m.RequestId == replyConsultationSupportingDocumentDto.RequestId)
                  .FirstOrDefaultAsync();
            entityToUpdate.Request.RequestStatus = replyConsultationSupportingDocumentDto.RequestStatus;

            await db.SaveChangesAsync();

            return mapper.Map<ConsultationSupportingDocumentRequestDto>(entityToUpdate);
        }

        public async Task EditAsync(ConsultationSupportingDocumentRequestDto ConsultationSupportingDocumentDto)
        {
            var entityToUpdate = await db.ConsultationSupportingDocuments
                .Include(d => d.Request)
                .Where(m => m.RequestId == ConsultationSupportingDocumentDto.RequestId && m.ConsultationId == ConsultationSupportingDocumentDto.ConsultationId)
                .FirstOrDefaultAsync();

            entityToUpdate.Request.Note = ConsultationSupportingDocumentDto.Request.Note;
            entityToUpdate.Request.RequestStatus = RequestStatuses.Modified;

            entityToUpdate.Request.UpdatedBy = CurrentUser.UserId;
            entityToUpdate.Request.UpdatedOn = DateTime.Now;
            await db.SaveChangesAsync();

            mapper.Map(entityToUpdate, ConsultationSupportingDocumentDto);
        }


    }
}
