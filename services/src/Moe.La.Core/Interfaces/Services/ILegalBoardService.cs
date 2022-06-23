using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Services
{
    public interface ILegalBoardService
    {
        Task<ReturnResult<QueryResultDto<LegalBoardListItemDto>>> GetAllAsync(LegalBoardQueryObject queryObject);

        Task<ReturnResult<LegalBoardDetailsDto>> GetAsync(int id);

        Task<ReturnResult<LegalBoardDto>> AddAsync(LegalBoardDto model);

        Task<ReturnResult<LegalBoardDto>> EditAsync(LegalBoardDto model);

        Task<ReturnResult<bool>> RemoveAsync(int id);

        Task<ReturnResult<bool>> ChangeStatusAsync(int id, int isActive);

        Task<ReturnResult<ICollection<LegalBoardSimpleDto>>> GetLegalBoard();

        #region legal-board-memo

        Task<ReturnResult<LegalBoardMemoDto>> AddLegalBoardMemoAsync(LegalBoardMemoDto model);

        Task<ReturnResult<LegalBoardMemoDto>> EditLegalBoardMemoAsync(LegalBoardMemoDto model);

        #endregion

        #region legal-board-member
        Task<ReturnResult<QueryResultDto<LegalBoardMemberHistoryDto>>> GetLegalBoardMemberHistory(LegalBoardMemberQueryObject queryObject);

        Task<ReturnResult<List<LegalBoradMemberDto>>> GetLegalBoardMembers(int legalMemoId);

        #endregion

        #region board-meeting
        Task<ReturnResult<QueryResultDto<BoardMeetingListItemDto>>> GetAllBoardMeetingAsync(BoardMeetingQueryObject queryObject);

        Task<ReturnResult<BoardMeetingDetailsDto>> GetBoardMeetingAsync(int id);
        Task<ReturnResult<BoardMeetingDetailsDto>> GetMeetingByBoardAndMemoAsync(int boardId, int memoId);

        Task<ReturnResult<BoardMeetingDto>> AddBoardMeetingAsync(BoardMeetingDto model);

        Task<ReturnResult<BoardMeetingDto>> EditBoardMeetingAsync(BoardMeetingDto model);
        #endregion 
    }
}
