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
    public class ExportCaseJudgmentRequestRepository : RepositoryBase, IExportCaseJudgmentRequestRepository
    {

        public ExportCaseJudgmentRequestRepository(LaDbContext commandDb, IMapper mapperConfig, IUserProvider userProvider)
            : base(commandDb, mapperConfig, userProvider)
        {
        }

        public async Task<ExportCaseJudgmentRequestListItemDto> GetAsync(int id)
        {
            var entity = await db.ExportCaseJudgmentRequests
                .Include(m => m.Request)
                  .ThenInclude(m => m.RequestTransactions)
                    .ThenInclude(mm => mm.CreatedByUser)
                     .ThenInclude(ur => ur.UserRoles)
                      .ThenInclude(r => r.Role)
                .Include(m => m.Request)
                  .ThenInclude(mm => mm.CreatedByUser)
                .Include(m => m.Request)
                    .ThenInclude(r => r.Letter)
                .Include(mm => mm.Case)
                  .ThenInclude(mm => mm.CaseRule)
                .Include(m => m.History)
                  .ThenInclude(mm => mm.Request)
                     .ThenInclude(mm => mm.CreatedByUser)

                .Where(m => m.Id == id).FirstOrDefaultAsync();

            return mapper.Map<ExportCaseJudgmentRequestListItemDto>(entity);
        }

        public async Task<ExportCaseJudgmentRequestDetailsDto> AddAsync(ExportCaseJudgmentRequestDto exportCaseJudgmentRequestDto)
        {
            var requestToAdd = new ExportCaseJudgmentRequest()
            {
                Request = new Request()
                {
                    RequestType = RequestTypes.RequestExportCaseJudgment,
                    RequestStatus = exportCaseJudgmentRequestDto.Request.RequestStatus,
                    ReceiverRoleId = ApplicationRolesConstants.GeneralSupervisor.Code,
                    SendingType = SendingTypes.Role,
                    IsExportable = true,
                    CreatedBy = CurrentUser.UserId,
                    CreatedOn = DateTime.Now,
                    Note = exportCaseJudgmentRequestDto.Request.Note

                },
                CaseId = exportCaseJudgmentRequestDto.CaseId,
            };

            if (!string.IsNullOrEmpty(exportCaseJudgmentRequestDto.Request.Letter.Text))
            {
                requestToAdd.Request.Letter = new RequestLetter()
                {
                    Text = exportCaseJudgmentRequestDto.Request.Letter.Text
                };
            }

            await db.ExportCaseJudgmentRequests.AddAsync(requestToAdd);
            await db.SaveChangesAsync();

            return mapper.Map<ExportCaseJudgmentRequestDetailsDto>(requestToAdd);
        }

        public async Task<ExportCaseJudgmentRequestDetailsDto> EditAsync(ExportCaseJudgmentRequestDto exportCaseJudgmentRequestDto)
        {
            var entityToUpdate = db.ExportCaseJudgmentRequests.Include(d => d.Request).ThenInclude(r => r.Letter).Where(d => d.Id == exportCaseJudgmentRequestDto.Id).FirstOrDefault();

            entityToUpdate.Request.RequestStatus = exportCaseJudgmentRequestDto.Request.RequestStatus;
            entityToUpdate.Request.Note = exportCaseJudgmentRequestDto.Request.Note;

            if (!string.IsNullOrEmpty(exportCaseJudgmentRequestDto.Request.Letter.Text))
            {
                entityToUpdate.Request.Letter = new RequestLetter()
                {
                    Text = exportCaseJudgmentRequestDto.Request.Letter.Text
                };
            }

            await db.SaveChangesAsync();

            return mapper.Map<ExportCaseJudgmentRequestDetailsDto>(entityToUpdate);
        }

        public async Task<ExportCaseJudgmentRequestDto> ReplyExportCaseJudgmentRequestAsync(ReplyExportCaseJudgmentRequestDto replyExportCaseJudgmentRequestDto)
        {
            var entityToUpdate = await db.ExportCaseJudgmentRequests
                  .Include(m => m.Request)
                  .Include(m => m.Case)
                      .ThenInclude(m => m.CaseRule)
                  .Where(m => m.Id == replyExportCaseJudgmentRequestDto.Id)
                  .FirstOrDefaultAsync();
            if (replyExportCaseJudgmentRequestDto.RequestStatus != RequestStatuses.New)
            {
                entityToUpdate.ReplyNote = replyExportCaseJudgmentRequestDto.ReplyNote;
                entityToUpdate.ReplyDate = DateTime.Now;
            }
            entityToUpdate.Request.RequestStatus = replyExportCaseJudgmentRequestDto.RequestStatus;

            if (replyExportCaseJudgmentRequestDto.RequestStatus == RequestStatuses.Approved)
            {
                //entityToUpdate.Case.Status = CaseStatuses.ClosedCase;
                //entityToUpdate.Case.CaseClosingType = replyCaseClosingRequestDto.CaseClosingType;
                entityToUpdate.Case.CaseRule.ExportRefNo = replyExportCaseJudgmentRequestDto.ExportRefNo;
                entityToUpdate.Case.CaseRule.ExportRefDate = replyExportCaseJudgmentRequestDto.ExportRefDate;
                //entityToUpdate.Case.CloseDate = DateTime.Now;
            }
            entityToUpdate.TransactionNumberInAdministrativeCommunications = replyExportCaseJudgmentRequestDto.TransactionNumberInAdministrativeCommunications;
            entityToUpdate.TransactionDateInAdministrativeCommunications = replyExportCaseJudgmentRequestDto.TransactionDateInAdministrativeCommunications;

            db.Update(entityToUpdate);
            await db.SaveChangesAsync();

            return mapper.Map<ExportCaseJudgmentRequestDto>(entityToUpdate);
        }

        public async Task<ExportCaseJudgmentRequestForPrintDto> GetForPrintAsync(int id)
        {
            var entity = await db.ExportCaseJudgmentRequests
                .Include(m => m.Request)
                    .ThenInclude(r => r.Letter)

                .Where(m => m.Id == id).FirstOrDefaultAsync();

            var model = mapper.Map<ExportCaseJudgmentRequestForPrintDto>(entity);

            return model;
        }


    }
}
