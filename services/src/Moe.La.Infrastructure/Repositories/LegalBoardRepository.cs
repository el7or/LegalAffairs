using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
    public class LegalBoardRepository : RepositoryBase, ILegalBoardRepository
    {
        public LegalBoardRepository(LaDbContext context, IMapper mapperConfig, IUserProvider userProvider)
            : base(context, mapperConfig, userProvider)
        {
        }

        public async Task<QueryResultDto<LegalBoardListItemDto>> GetAllAsync(LegalBoardQueryObject queryObject)
        {
            var result = new QueryResult<LegalBoard>();

            var query = db.LegalBoards
                .Include(l => l.LegalBoardMembers).ThenInclude(m => m.User)
                .OrderBy(n => n.Name)
                .AsNoTracking()
                .AsQueryable();

            var columnsMap = new Dictionary<string, Expression<Func<LegalBoard, object>>>()
            {
                ["name"] = v => v.Name,
                ["createdOn"] = v => v.CreatedOn,
                ["legalBoardTypeId"] = v => v.LegalBoardTypeId,
                ["status"] = v => v.StatusId,
            };

            query = query.ApplySorting(queryObject, columnsMap);

            result.TotalItems = await query.CountAsync();

            query = query.ApplyPaging(queryObject);

            result.Items = await query.ToListAsync();

            return mapper.Map<QueryResult<LegalBoard>, QueryResultDto<LegalBoardListItemDto>>(result);
        }

        public async Task<LegalBoardDetailsDto> GetAsync(int id)
        {
            var legalBoard = await db.LegalBoards
                .Include(l => l.LegalBoardMembers).ThenInclude(m => m.User)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);

            return mapper.Map<LegalBoard, LegalBoardDetailsDto>(legalBoard);
        }

        public async Task AddAsync(LegalBoardDto legalBoardDto)
        {
            var entityToAdd = mapper.Map<LegalBoard>(legalBoardDto);

            entityToAdd.CreatedBy = CurrentUser.UserId;
            entityToAdd.CreatedOn = DateTime.Now;
            entityToAdd.StatusId = LegalBoardStatuses.Activated;
            entityToAdd.LegalBoardTypeId = legalBoardDto.TypeId;

            entityToAdd.LegalBoardMembers.ToList().ForEach(m =>
            {
                m.CreatedOn = DateTime.Now;
                m.CreatedBy = CurrentUser.UserId;
            });

            await db.LegalBoards.AddAsync(entityToAdd);
            await db.SaveChangesAsync();
            mapper.Map(entityToAdd, legalBoardDto);
            //mapper.Map(entityToAdd.BoardMembers, legalBoardDto.LegalBoardMembers);

        }
        //Check if there is Major legal board
        public async Task<bool> CheckMajorLegalBoardAsync(int? legalBoardId)
        {
            var legalBoard = new LegalBoard();
            if (legalBoardId != null)
                legalBoard = await db.LegalBoards
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.LegalBoardTypeId == LegalBoardTypes.Major && b.Id != legalBoardId.Value);
            else
                legalBoard = await db.LegalBoards
               .AsNoTracking()
               .FirstOrDefaultAsync(b => b.LegalBoardTypeId == LegalBoardTypes.Major);
            if (legalBoard == null)
                return false;
            return true;
        }

        //Add   members history to record join periods.
        public async Task AddLegalBoardMemberHistoryAsync(List<LegalBoradMemberDto> legalBoardMembers, int legalBoardId)
        {
            foreach (var member in legalBoardMembers)
            {
                var entityToAdd = mapper.Map<LegalBoradMemberDto, LegalBoardMemberHistory>(member);
                entityToAdd.LegalBoardId = legalBoardId;
                entityToAdd.CreatedBy = CurrentUser.UserId;

                var entityHistoryToUpdate = db.LegalBoardMemberHistory
                .Where(h => h.UserId == member.UserId && h.LegalBoardId == legalBoardId && h.EndDate == null)
                .FirstOrDefault();

                if (entityHistoryToUpdate != null && entityHistoryToUpdate.UserId == member.UserId)
                {
                    if (entityHistoryToUpdate.MembershipType != member.MembershipType)
                    {
                        entityHistoryToUpdate.EndDate = DateTime.Now;
                        entityHistoryToUpdate.IsActive = false;
                        entityHistoryToUpdate.UpdatedBy = CurrentUser.UserId;
                        entityHistoryToUpdate.UpdatedOn = DateTime.Now;

                        if (member.IsActive)
                            await db.LegalBoardMemberHistory.AddAsync(entityToAdd);
                    }
                }
                else
                {
                    if (member.IsActive)
                        await db.LegalBoardMemberHistory.AddAsync(entityToAdd);
                }
            }

            await db.SaveChangesAsync();
        }

        public async Task EditAsync(LegalBoardDto legalBoardDto)
        {
            var entityToUpdate = await db.LegalBoards.Where(l => l.Id == legalBoardDto.Id)
                .Include(l => l.LegalBoardMembers)
                .FirstOrDefaultAsync();


            mapper.Map(legalBoardDto, entityToUpdate);

            entityToUpdate.LegalBoardMembers.ToList().ForEach(m =>
            {

                if (m.Id == 0)
                    m.CreatedBy = CurrentUser.UserId;
                else
                    m.UpdatedBy = CurrentUser.UserId;

            });

            entityToUpdate.UpdatedBy = CurrentUser.UserId;
            entityToUpdate.UpdatedOn = DateTime.Now;
            entityToUpdate.StatusId = LegalBoardStatuses.Activated;
            entityToUpdate.LegalBoardTypeId = legalBoardDto.TypeId;


            await db.SaveChangesAsync();
            mapper.Map(entityToUpdate, legalBoardDto);
        }

        public async Task RemoveAsync(int id)
        {
            var entityToDelete = await db.LegalBoards.FindAsync(id);

            entityToDelete.UpdatedBy = CurrentUser.UserId;
            entityToDelete.UpdatedOn = DateTime.Now;
            entityToDelete.IsDeleted = true;

            await db.SaveChangesAsync();
        }

        public async Task<Guid?> GetMemberUserIdAsync(int memberId)
        {
            var entity = await db.LegalBoardMembers.Include(m => m.User).Where(m => m.Id == memberId).FirstOrDefaultAsync();

            return entity != null ? entity.User.Id : null;

        }
        public async Task ChangeStatusAsync(int id, int isActive)
        {
            var entityToUpdate = await db.LegalBoards.FindAsync(id);

            entityToUpdate.UpdatedBy = CurrentUser.UserId;
            entityToUpdate.UpdatedOn = DateTime.Now;
            entityToUpdate.StatusId = (LegalBoardStatuses)Enum.ToObject(typeof(LegalBoardStatuses), isActive);

            await db.SaveChangesAsync();
        }

        public async Task<ICollection<LegalBoardSimpleDto>> GetLegalBoard()
        {
            var legalBoard = await db.LegalBoards.Where(l => l.LegalBoardTypeId == LegalBoardTypes.Secondary
            && l.StatusId == LegalBoardStatuses.Activated && l.LegalBoardMembers.Any(m => m.MembershipType == LegalBoardMembershipTypes.Head && m.IsActive)).ToListAsync();

            return mapper.Map<ICollection<LegalBoard>, ICollection<LegalBoardSimpleDto>>(legalBoard);
        }

        public async Task<int> GetMajorLegalBoardId()
        {
            var MajorLegalBoard = await db.LegalBoards.FirstOrDefaultAsync(b => b.LegalBoardTypeId == LegalBoardTypes.Major);

            return (MajorLegalBoard != null) ? MajorLegalBoard.Id : 0;
        }

        #region legal-board-memo

        public async Task AddLegalBoardMemoAsync(LegalBoardMemoDto legalBoardMemoDto)
        {
            var entityToAdd = mapper.Map<LegalBoardMemo>(legalBoardMemoDto);

            entityToAdd.CreatedOn = DateTime.Now;
            entityToAdd.CreatedBy = CurrentUser.UserId;

            await db.LegalBoardMemos.AddAsync(entityToAdd);
            await db.SaveChangesAsync();

            legalBoardMemoDto.Id = entityToAdd.Id;
        }

        public async Task EditLegalBoardMemoAsync(LegalBoardMemoDto legalBoardMemoDto)
        {
            var entityToUpdate = await db.LegalBoardMemos
                .Where(b => b.Id == legalBoardMemoDto.Id)
                .FirstOrDefaultAsync();

            mapper.Map(legalBoardMemoDto, entityToUpdate);

            entityToUpdate.UpdatedBy = CurrentUser.UserId;
            entityToUpdate.UpdatedOn = DateTime.Now;

            await db.SaveChangesAsync();
        }

        #endregion

        #region legal-board-members

        public async Task<QueryResultDto<LegalBoardMemberHistoryDto>> GetLegalBoardMemberHistory(LegalBoardMemberQueryObject queryObject)
        {
            var result = new QueryResult<LegalBoardMemberHistory>();
            var query = db.LegalBoardMemberHistory
                .AsNoTracking()
                .AsQueryable();

            if (queryObject.LegalBoardId != null)
                query = query.Where(n => n.LegalBoardId == queryObject.LegalBoardId);

            if (queryObject.UserId != null)
                query = query.Where(n => n.UserId == queryObject.UserId);

            var columnsMap = new Dictionary<string, Expression<Func<LegalBoardMemberHistory, object>>>()
            {
                ["startDate"] = v => v.StartDate,
            };

            query = query.ApplySorting(queryObject, columnsMap);

            result.TotalItems = await query.CountAsync();

            query = query.ApplyPaging(queryObject);

            result.Items = await query.ToListAsync();
            return mapper.Map<QueryResult<LegalBoardMemberHistory>, QueryResultDto<LegalBoardMemberHistoryDto>>(result);
        }

        public async Task<List<LegalBoradMemberDto>> GetLegalBoardMembers(int boardId)
        {
            var legalBoardMembers = await db.LegalBoardMembers
                .Include(m => m.LegalBoard)
                .Include(l => l.User)
                .Where(m => m.IsActive == true && m.LegalBoardId == boardId)
                .ToListAsync();

            return mapper.Map<List<LegalBoradMemberDto>>(legalBoardMembers);
        }
        #endregion



    }
}
