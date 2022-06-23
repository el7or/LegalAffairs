using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Repositories
{
    public interface ILegalBoardRepository
    {
        Task<QueryResultDto<LegalBoardListItemDto>> GetAllAsync(LegalBoardQueryObject queryObject);

        Task<LegalBoardDetailsDto> GetAsync(int id);

        Task AddAsync(LegalBoardDto legalBoardDto);

        Task EditAsync(LegalBoardDto legalBoardDto);

        Task<Guid?> GetMemberUserIdAsync(int memberId);

        Task<bool> CheckMajorLegalBoardAsync(int? legalBoardId);

        Task AddLegalBoardMemberHistoryAsync(List<LegalBoradMemberDto> legalBoardMembers, int legalBoardId);

        Task RemoveAsync(int id);

        Task ChangeStatusAsync(int id, int isActive);

        Task<ICollection<LegalBoardSimpleDto>> GetLegalBoard();

        Task AddLegalBoardMemoAsync(LegalBoardMemoDto legalBoardMemoDto);

        Task EditLegalBoardMemoAsync(LegalBoardMemoDto legalBoardMemoDto);

        Task<int> GetMajorLegalBoardId();

        #region legal-board-member

        Task<QueryResultDto<LegalBoardMemberHistoryDto>> GetLegalBoardMemberHistory(LegalBoardMemberQueryObject queryObject);

        Task<List<LegalBoradMemberDto>> GetLegalBoardMembers(int legalMemoId);

        #endregion


    }
}
