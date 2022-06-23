using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Services
{
    public interface ILegalMemoService
    {
        #region LegalMemo
        Task<ReturnResult<QueryResultDto<LegalMemoListItemDto>>> GetAllAsync(LegalMemoQueryObject queryObject);

        Task<ReturnResult<LegalMemoDetailsDto>> GetAsync(int id);

        Task<ReturnResult<LegalMemoForPrintDetailsDto>> GetForPrintAsync(int id, int hearingId);

        Task<ReturnResult<LegalMemoDto>> AddAsync(LegalMemoDto legalMemoDto);

        Task<ReturnResult<LegalMemoDto>> EditAsync(LegalMemoDto legalMemoDto);

        Task<ReturnResult<bool>> RemoveAsync(DeletionLegalMemoDto deletionLegalMemoDto);

        Task<ReturnResult<bool>> ChangeLegalMemoStatusAsync(int id, int legalMemoStatusId);

        Task<ReturnResult<bool>> RaisToAllBoardsHeadAsync(int id);

        Task<ReturnResult<bool>> ApproveAsync(int id);

        Task<ReturnResult<bool>> RejectAsync(int id, string note);

        Task<ReturnResult<bool>> ReadLegalMemoAsync(int id);

        Task<ReturnResult<ICollection<LegalMemoListItemDto>>> GetAllLegalMemoByCaseIdAsync(int CaseID);
        #endregion

        #region LegalMemoNotes

        Task<ReturnResult<QueryResultDto<LegalMemoNoteListItemDto>>> GetNotesAllAsync(LegalMemoNoteQueryObject queryObject);

        Task<ReturnResult<LegalMemoNoteListItemDto>> GetNoteAsync(int id);

        Task<ReturnResult<LegalMemoNoteDto>> AddNoteAsync(LegalMemoNoteDto legalMemoNoteDto);

        Task<ReturnResult<LegalMemoNoteDto>> EditNoteAsync(LegalMemoNoteDto legalMemoNoteDto);

        Task<ReturnResult<bool>> RemoveNoteAsync(int id);

        #endregion

        #region LegalMemosHistory

        Task<ReturnResult<QueryResultDto<LegalMemosHistoryListItemDto>>> GetHistoryAllAsync(LegalMemoQueryObject queryObject);

        #endregion
    }
}
