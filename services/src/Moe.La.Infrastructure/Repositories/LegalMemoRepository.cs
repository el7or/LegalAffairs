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

    public class LegalMemoRepository : RepositoryBase, ILegalMemoRepository
    {
        public LegalMemoRepository(LaDbContext context, IMapper mapperConfig, IUserProvider userProvider)
            : base(context, mapperConfig, userProvider)
        {
        }

        public async Task<QueryResultDto<LegalMemoListItemDto>> GetAllAsync(LegalMemoQueryObject queryObject)
        {
            var result = new QueryResult<LegalMemo>();

            var query = db.LegalMemos
                .Include(l => l.LegalBoardMemos)
                .Include(l => l.InitialCase)
                .ThenInclude(c => c.Researchers)
                .ThenInclude(r => r.Researcher)
                .Include(n => n.SecondSubCategory)
                .Include(n => n.CreatedByUser)
                .Include(n => n.UpdatedByUser)
                .OrderBy(n => n.Name)
                .AsNoTracking()
                .AsQueryable().IgnoreQueryFilters();

            IQueryable<LegalMemo> finalQuery = null;

            #region fiter hearings by roles

            if (CurrentUser.Roles.Contains(ApplicationRolesConstants.LegalConsultant.Name))
            {

                //return memos in case of consultant & board head & board   members roles              
                var researchers = db.ResearcherConsultants
                        .Where(u => u.ConsultantId == CurrentUser.UserId && u.IsActive == true)
                        .Select(x => x.ResearcherId).ToList();
                var newQuery = query.Where(x => x.InitialCase.Researchers.All(cr => researchers.Any(r => cr.ResearcherId == r)) && x.Status == LegalMemoStatuses.RaisingConsultant);
                if (finalQuery == null) finalQuery = newQuery; else finalQuery = finalQuery.Union(newQuery);
            }

            if (CurrentUser.Roles.Contains(ApplicationRolesConstants.LegalResearcher.Name))
            {
                var newQuery = query.Where(x => x.InitialCase.Researchers.Any(cr => cr.ResearcherId == CurrentUser.UserId && !cr.IsDeleted) || x.Status == LegalMemoStatuses.Approved);
                if (finalQuery == null) finalQuery = newQuery; else finalQuery = finalQuery.Union(newQuery);

            }

            if (CurrentUser.Roles.Contains(ApplicationRolesConstants.SubBoardHead.Name))
            {
                var legalBoards = db.LegalBoards.Include(b => b.LegalBoardMembers).Where(m => m.LegalBoardMembers.Any(mm => mm.UserId == CurrentUser.UserId) && m.LegalBoardTypeId == LegalBoardTypes.Secondary);

                // use UpdatedBy to get memo for user who reject it
                var newQuery = query.Where(x => (x.Status == LegalMemoStatuses.RaisingSubBoardHead || (x.Status == LegalMemoStatuses.Rejected && x.UpdatedBy == CurrentUser.UserId)) && legalBoards.Any(mmm => mmm.Id == x.LegalBoardMemos.OrderBy(m => m.CreatedOn).LastOrDefault().LegalBoardId));
                if (finalQuery == null) finalQuery = newQuery; else finalQuery = finalQuery.Union(newQuery);

            }

            if (CurrentUser.Roles.Contains(ApplicationRolesConstants.MainBoardHead.Name))
            {
                var majorlegalBoards = db.LegalBoards.Include(b => b.LegalBoardMembers).Where(m => m.LegalBoardMembers.Any(mm => mm.UserId == CurrentUser.UserId && mm.MembershipType == LegalBoardMembershipTypes.Head) && m.LegalBoardTypeId == LegalBoardTypes.Major);
                // use UpdatedBy to get memo for user who reject it
                var newQuery = query.Where(x => (x.Status == LegalMemoStatuses.RaisingMainBoardHead || (x.Status == LegalMemoStatuses.Rejected && x.UpdatedBy == CurrentUser.UserId)) && majorlegalBoards.Any(mmm => mmm.Id == x.LegalBoardMemos.OrderBy(m => m.CreatedOn).LastOrDefault().LegalBoardId));
                if (finalQuery == null) finalQuery = newQuery; else finalQuery = finalQuery.Union(newQuery);

            }

            if (CurrentUser.Roles.Contains(ApplicationRolesConstants.AllBoardsHead.Name))
            {
                // use UpdatedBy to get memo for user who reject it
                var newQuery = query.Where(x => x.Status == LegalMemoStatuses.RaisingAllBoardsHead || (x.Status == LegalMemoStatuses.Rejected && x.UpdatedBy == CurrentUser.UserId));
                if (finalQuery == null) finalQuery = newQuery; else finalQuery = finalQuery.Union(newQuery);

            }


            #endregion

            // if final query equal null
            if (finalQuery == null)
                finalQuery = query.Where(x => false);

            if (!string.IsNullOrEmpty(queryObject.Status))
            {
                if (queryObject.Status != "0") // we set all value to 0
                {
                    //return memo which after status filter
                    var status = queryObject.Status.Split(new char[] { ',' });
                    query = query.Where(s => status.Contains(((int)s.Status).ToString()));
                }
            }

            if (!string.IsNullOrEmpty(queryObject.SearchText))
            {
                query = query.Where(m => m.Name.Contains(queryObject.SearchText));
            }

            if (!string.IsNullOrEmpty(queryObject.Name))
            {
                query = query.Where(m => m.Name.Contains(queryObject.Name));
            }

            if (queryObject.SecondSubCategoryId.HasValue)
            {
                query = query.Where(m => m.SecondSubCategoryId == queryObject.SecondSubCategoryId);

            }
            if (queryObject.Type.HasValue)
            {
                query = query.Where(m => m.Type == queryObject.Type);

            }

            if (queryObject.CreatedBy != null)
                query = query.Where(c => c.CreatedBy == queryObject.CreatedBy);

            if (!string.IsNullOrEmpty(queryObject.ApprovalFrom))
            {
                query = query.Where(p => p.UpdatedOn.Value.Date >= DateTime.Parse(queryObject.ApprovalFrom).Date);
            }

            if (!string.IsNullOrEmpty(queryObject.ApprovalTo))
            {
                DateTime to = Convert.ToDateTime(queryObject.ApprovalTo);
                query = query.Where(p => p.UpdatedOn.Value.Date <= to.Date);
            }

            if (!string.IsNullOrEmpty(queryObject.UpdatedOn))
            {
                query = query.Where(p => p.UpdatedOn.Value.Date >= DateTime.Parse(queryObject.UpdatedOn).Date);
            }

            if (!string.IsNullOrEmpty(queryObject.CreatedOn))
            {
                DateTime to = Convert.ToDateTime(queryObject.CreatedOn);
                query = query.Where(p => p.UpdatedOn.Value.Date <= to.Date);
            }

            var columnsMap = new Dictionary<string, Expression<Func<LegalMemo, object>>>()
            {
                ["id"] = v => v.Id,
                ["name"] = v => v.Name,
                ["status"] = v => v.Status,
                ["updatedOn"] = v => v.UpdatedOn,
                ["createdOn"] = v => v.CreatedOn,
                ["raisedOn"] = v => v.RaisedOn,
                ["createdByUser"] = v => v.CreatedByUser.FirstName + " " + v.CreatedByUser.LastName
            };

            query = query.ApplySorting(queryObject, columnsMap);

            result.TotalItems = await query.CountAsync();

            query = query.ApplyPaging(queryObject);

            result.Items = await query.ToListAsync();

            return mapper.Map<QueryResult<LegalMemo>, QueryResultDto<LegalMemoListItemDto>>(result);
        }

        public async Task<LegalMemoDetailsDto> GetAsync(int id)
        {
            var legalMemo = await db.LegalMemos
                 .Include(m => m.SecondSubCategory)
                 .Include(l => l.InitialCase)
                    .ThenInclude(c => c.Researchers)
                       .ThenInclude(r => r.Researcher)
                 .Include(m => m.CreatedByUser)
                 .Include(m => m.UpdatedByUser)
                 .Include(m => m.LegalBoardMemos)
                 .Include(b => b.BoardMeetings)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);

            return mapper.Map<LegalMemoDetailsDto>(legalMemo);
        }
        public async Task<LegalMemoForPrintDetailsDto> GetForPrintAsync(int id, int hearingId)
        {
            var hearingLegalMemo = await db.Hearings
                .Include(m => m.HearingLegalMemoReviewRequests)
                    .ThenInclude(m => m.LegalMemo)
                .Include(m => m.Case)
                    .ThenInclude(c => c.Parties)
                        .ThenInclude(c => c.Party)
                .Include(m => m.Case)
                    .ThenInclude(c => c.Researchers)
                        .ThenInclude(r => r.Researcher)
                .Include(m => m.Case)
                    .ThenInclude(c => c.Court)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == hearingId);

            return mapper.Map<LegalMemoForPrintDetailsDto>(hearingLegalMemo);
        }

        public async Task<LegalMemosHistoryDto> GetToHistoryAsync(int id)
        {
            var legalMemo = await db.LegalMemos
               .AsNoTracking()
               .FirstOrDefaultAsync(s => s.Id == id);

            return mapper.Map<LegalMemosHistoryDto>(legalMemo);
        }

        public async Task AddAsync(LegalMemoDto legalMemoDto)
        {
            var entityToAdd = mapper.Map<LegalMemo>(legalMemoDto);

            if (legalMemoDto.Status == LegalMemoStatuses.RaisingConsultant)
                entityToAdd.RaisedOn = DateTime.Now;

            entityToAdd.CreatedBy = CurrentUser.UserId;
            entityToAdd.CreatedOn = DateTime.Now;

            await db.LegalMemos.AddAsync(entityToAdd);
            await db.SaveChangesAsync();

            mapper.Map(entityToAdd, legalMemoDto);
        }

        public async Task EditAsync(LegalMemoDto legalMemoDto)
        {
            var entityToUpdate = await db.LegalMemos
                .Include(c => c.SecondSubCategory)
                .FirstOrDefaultAsync(s => s.Id == legalMemoDto.Id);
            mapper.Map(legalMemoDto, entityToUpdate);
            entityToUpdate.UpdatedBy = CurrentUser.UserId;
            entityToUpdate.UpdatedOn = DateTime.Now;
            entityToUpdate.RaisedOn = legalMemoDto.Status == LegalMemoStatuses.RaisingConsultant ? DateTime.Now : entityToUpdate.RaisedOn;
            await db.SaveChangesAsync();
            mapper.Map(entityToUpdate, legalMemoDto);
        }

        public async Task RemoveAsync(DeletionLegalMemoDto deletionLegalMemoDto)
        {
            var entityToDelete = await db.LegalMemos.FindAsync(deletionLegalMemoDto.Id);

            entityToDelete.UpdatedBy = CurrentUser.UserId;
            entityToDelete.UpdatedOn = DateTime.Now;
            entityToDelete.IsDeleted = true;
            entityToDelete.DeletionReason = deletionLegalMemoDto.DeletionReason;

            await db.SaveChangesAsync();
        }

        public async Task ChangeLegalMemoStatusAsync(int legalMemoId, int legalMemoStatusId)
        {
            var entityToUpdate = await db.LegalMemos
                .FirstOrDefaultAsync(s => s.Id == legalMemoId);
            if ((LegalMemoStatuses)legalMemoStatusId == LegalMemoStatuses.RaisingConsultant
                || (LegalMemoStatuses)legalMemoStatusId == LegalMemoStatuses.RaisingSubBoardHead)
                entityToUpdate.RaisedOn = DateTime.Now;
            entityToUpdate.Status = (LegalMemoStatuses)legalMemoStatusId;
            entityToUpdate.UpdatedBy = CurrentUser.UserId;
            entityToUpdate.UpdatedOn = DateTime.Now;

            await db.SaveChangesAsync();

        }

        public async Task ReadLegalMemoAsync(int legalMemoId, int reviewNumber)
        {
            var entityToUpdate = await db.LegalMemos
                .FirstOrDefaultAsync(s => s.Id == legalMemoId);

            entityToUpdate.IsRead = true;
            entityToUpdate.ReviewNumber = reviewNumber;
            entityToUpdate.UpdatedBy = CurrentUser.UserId;
            entityToUpdate.UpdatedOn = DateTime.Now;

            await db.SaveChangesAsync();

        }


        public async Task<ICollection<LegalMemoListItemDto>> GetAllLegalMemoByCaseIdAsync(int caseId)
        {
            var query = db.LegalMemos
                 .Include(u => u.CreatedByUser)
                    .Include(a => a.UpdatedByUser)
                .OrderBy(n => n.Name)
                .Where(l => l.InitialCaseId == caseId)
                .AsNoTracking()
                .AsQueryable();


            var result = await query.ToListAsync();

            return mapper.Map<ICollection<LegalMemo>, ICollection<LegalMemoListItemDto>>(result);
        }

    }
}
