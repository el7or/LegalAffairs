using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
    public class CaseSupportingDocumentRequestRepository : RepositoryBase, ICaseSupportingDocumentRequestRepository
    {

        public CaseSupportingDocumentRequestRepository(LaDbContext commandDb, IMapper mapperConfig, IUserProvider userProvider)
            : base(commandDb, mapperConfig, userProvider)
        {
        }

        public async Task<CaseSupportingDocumentRequestListItemDto> GetAsync(int id)
        {
            var entity = await db.DocumentRequests
                .Include(m => m.Request)
                  .ThenInclude(m => m.Letter)
                .Include(m => m.Request)
                  .ThenInclude(m => m.RequestTransactions)
                    .ThenInclude(mm => mm.CreatedByUser)
                     .ThenInclude(ur => ur.UserRoles)
                      .ThenInclude(r => r.Role)
                .Include(m => m.Request)
                  .ThenInclude(mm => mm.CreatedByUser)
               .Include(m => m.History)
                  .ThenInclude(mm => mm.Documents)
                .Include(m => m.History)
                    .ThenInclude(mm => mm.Request)
                       .ThenInclude(mm => mm.CreatedByUser)
                .Include(m => m.History)
                   .ThenInclude(mm => mm.Request)
                       .ThenInclude(mm => mm.UpdatedByUser)
                .Include(m => m.Documents)
                .Include(r => r.Case)
                .Include(m => m.ConsigneeDepartment)
                .Include(m => m.Parent)
                   .ThenInclude(m => m.Case)
                .Where(m => m.Id == id)
                .FirstOrDefaultAsync();

            var model = mapper.Map<CaseSupportingDocumentRequestListItemDto>(entity);
            model.AttachedLetterRequestCount = db.DocumentRequests.Where(r => r.ParentId == entity.Id).Count();

            if (model.AttachedLetterRequestCount > 0)
            {
                model.AttachedLetterRequestId = db.DocumentRequests
                    .FirstOrDefault(r => r.ParentId == entity.Id).Id;

                model.AttachedLetterRequestStatus = db.DocumentRequests
                    .Include(m => m.Request)
                    .FirstOrDefault(r => r.ParentId == entity.Id).Request.RequestStatus;
            }

            return model;
        }

        public async Task<AttachedLetterRequestDto> GetAttachedLetterRequestAsync(int id)
        {
            var entity = await db.DocumentRequests
                .Include(m => m.Request)
                  .ThenInclude(m => m.RequestTransactions)
                    .ThenInclude(mm => mm.CreatedByUser)
                     .ThenInclude(ur => ur.UserRoles)
                      .ThenInclude(r => r.Role)
                .Include(m => m.Request)
                  .ThenInclude(mm => mm.CreatedByUser)
                .Include(m => m.Request)
                  .ThenInclude(mm => mm.Letter)
                .Include(r => r.Parent)
                .ThenInclude(p => p.Case)
                .Where(m => m.Id == id).FirstOrDefaultAsync();

            var model = mapper.Map<AttachedLetterRequestDto>(entity);

            return model;
        }


        public async Task<CaseSupportingDocumentRequestForPrintDto> GetForPrintAsync(int id)
        {
            var entity = await db.DocumentRequests
                .Include(m => m.Request)
                .Include(r => r.Case)
                    .ThenInclude(r => r.Parties)
                    .ThenInclude(p => p.Party)
                .Include(m => m.Documents)
                .Where(m => m.Id == id).FirstOrDefaultAsync();

            var model = mapper.Map<CaseSupportingDocumentRequestForPrintDto>(entity);

            return model;
        }

        public async Task<CaseSupportingDocumentRequestDto> AddAsync(CaseSupportingDocumentRequestDto documentRequestDto)
        {
            var entityToAdd = mapper.Map<CaseSupportingDocumentRequest>(documentRequestDto);

            entityToAdd.Request.CreatedBy = CurrentUser.UserId;
            entityToAdd.Request.CreatedOn = DateTime.Now;

            var researcherConsultant = await db.ResearcherConsultants.Include(c => c.Researcher)
                .Where(m => m.ResearcherId == CurrentUser.UserId)
                .FirstOrDefaultAsync();

            entityToAdd.Request.ReceiverId = researcherConsultant.ConsultantId;
            entityToAdd.Request.ReceiverBranchId = researcherConsultant.Researcher.BranchId;
            entityToAdd.Request.ReceiverDepartmentId = (int)Departments.Litigation;
            entityToAdd.Request.SendingType = SendingTypes.User;
            entityToAdd.Request.RequestType = RequestTypes.RequestSupportingDocuments;
            entityToAdd.Request.RequestStatus = RequestStatuses.Draft;
            entityToAdd.Request.IsExportable = true;

            await db.DocumentRequests.AddAsync(entityToAdd);
            await db.SaveChangesAsync();

            return mapper.Map(entityToAdd, documentRequestDto);
        }

        public async Task<AttachedLetterRequestDto> AddAttachedLetterRequestAsync(AttachedLetterRequestDto attachedLetterRequestDto)
        {
            var entityToAdd = mapper.Map<CaseSupportingDocumentRequest>(attachedLetterRequestDto);

            entityToAdd.Request.CreatedBy = CurrentUser.UserId;
            entityToAdd.Request.CreatedOn = DateTime.Now;

            var researcherConsultant = await db.ResearcherConsultants
                .Include(r => r.Researcher)
                .Where(m => m.ResearcherId == CurrentUser.UserId)
                .FirstOrDefaultAsync();

            entityToAdd.Request.ReceiverId = researcherConsultant.ConsultantId;
            entityToAdd.Request.ReceiverBranchId = researcherConsultant.Researcher.BranchId;
            entityToAdd.Request.ReceiverDepartmentId = (int)Departments.Litigation;
            entityToAdd.Request.SendingType = SendingTypes.User;
            entityToAdd.Request.RequestType = RequestTypes.RequestAttachedLetter;
            entityToAdd.Request.IsExportable = true;

            entityToAdd.Request.Letter = mapper.Map<RequestLetter>(attachedLetterRequestDto.Request.Letter);


            await db.DocumentRequests.AddAsync(entityToAdd);
            await db.SaveChangesAsync();


            return mapper.Map(entityToAdd, attachedLetterRequestDto);
        }

        public async Task<AttachedLetterRequestDto> EditAttachedLetterRequestAsync(AttachedLetterRequestDto attachedLetterRequestDto)
        {
            var entityToUpdate = await db.DocumentRequests
                .Include(d => d.Request).ThenInclude(r => r.Letter)
                .Where(d => d.Id == attachedLetterRequestDto.Id).FirstOrDefaultAsync();

            entityToUpdate.Request.UpdatedBy = CurrentUser.UserId;
            entityToUpdate.Request.UpdatedOn = DateTime.Now;
            entityToUpdate.Request.Note = attachedLetterRequestDto.Request.Note;
            entityToUpdate.Request.RequestStatus = attachedLetterRequestDto.Request.RequestStatus;

            entityToUpdate.ConsigneeDepartmentId = attachedLetterRequestDto.consigneeDepartmentId;
            entityToUpdate.Request.Letter = mapper.Map<RequestLetter>(attachedLetterRequestDto.Request.Letter);

            await db.SaveChangesAsync();

            return mapper.Map(entityToUpdate, attachedLetterRequestDto);
        }

        public async Task<CaseSupportingDocumentRequestDto> ReplyDocumentRequestAsync(ReplyCaseSupportingDocumentRequestDto replyDocumentRequestDto)
        {
            var entityToUpdate = await db.DocumentRequests
                  .Include(m => m.Request)
                  .Where(m => m.Id == replyDocumentRequestDto.Id)
                  .FirstOrDefaultAsync();
            entityToUpdate.ReplyNote = replyDocumentRequestDto.ReplyNote;
            entityToUpdate.ReplyDate = DateTime.Now;
            entityToUpdate.Request.RequestStatus = replyDocumentRequestDto.RequestStatus;
            entityToUpdate.TransactionNumberInAdministrativeCommunications = replyDocumentRequestDto.TransactionNumberInAdministrativeCommunications;
            entityToUpdate.TransactionDateInAdministrativeCommunications = replyDocumentRequestDto.TransactionDateInAdministrativeCommunications;
            if (replyDocumentRequestDto.ConsigneeDepartmentId != null)
            {
                entityToUpdate.ConsigneeDepartmentId = replyDocumentRequestDto.ConsigneeDepartmentId;
            }

            await db.SaveChangesAsync();

            return mapper.Map<CaseSupportingDocumentRequestDto>(entityToUpdate);
        }

        public async Task EditAsync(CaseSupportingDocumentRequestDto documentRequestDto)
        {
            var entityToUpdate = await db.DocumentRequests.
                Include(d => d.Documents)
                .Include(d => d.Request)
                    .ThenInclude(d => d.Letter)
                .Where(d => d.Id == documentRequestDto.Id).FirstOrDefaultAsync();

            db.DocumentRequestItems.RemoveRange(entityToUpdate.Documents);

            var docs = documentRequestDto.Documents;
            entityToUpdate.Request.Note = documentRequestDto.Request.Note;
            entityToUpdate.Request.RequestStatus = documentRequestDto.Request.RequestStatus;
            //entityToUpdate.Request.Letter = mapper.Map<RequestLetter>(documentRequestDto.Request.Letter);

            entityToUpdate.ConsigneeDepartmentId = documentRequestDto.ConsigneeDepartmentId;

            foreach (var doc in docs)
            {
                entityToUpdate.Documents.Add(new CaseSupportingDocumentRequestItem
                {
                    CaseSupportingDocumentRequestId = entityToUpdate.Id,
                    Name = doc.Name
                });
            }

            entityToUpdate.Request.UpdatedBy = CurrentUser.UserId;
            entityToUpdate.Request.UpdatedOn = DateTime.Now;
            await db.SaveChangesAsync();

            mapper.Map(entityToUpdate, documentRequestDto);
        }

        public async Task RemoveAsync(int id)
        {
            var entityToDelete = await db.DocumentRequests.FindAsync(id);
            await db.SaveChangesAsync();
        }


    }
}
