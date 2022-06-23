using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Repositories
{
    public interface ILegalMemoRepository
    {
        Task<QueryResultDto<LegalMemoListItemDto>> GetAllAsync(LegalMemoQueryObject queryObject);

        Task<LegalMemoDetailsDto> GetAsync(int id);

        Task<LegalMemoForPrintDetailsDto> GetForPrintAsync(int id, int hearingId);

        Task<LegalMemosHistoryDto> GetToHistoryAsync(int id);

        Task AddAsync(LegalMemoDto legalMemoDto);

        Task EditAsync(LegalMemoDto legalMemoDto);

        Task RemoveAsync(DeletionLegalMemoDto deletionLegalMemoDto);

        Task ChangeLegalMemoStatusAsync(int legalMemoId, int legalMemoStatusId);

        Task ReadLegalMemoAsync(int legalMemoId, int reviewNumber);

        Task<ICollection<LegalMemoListItemDto>> GetAllLegalMemoByCaseIdAsync(int caseId);

    }
}
