using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Repositories
{
    public interface ILegalMemoNoteRepository
    {
        Task<QueryResultDto<LegalMemoNoteListItemDto>> GetAllAsync(LegalMemoNoteQueryObject queryObject);

        Task<LegalMemoNoteListItemDto> GetAsync(int id);

        Task AddAsync(LegalMemoNoteDto legalMemoNoteDto);

        Task EditAsync(LegalMemoNoteDto legalMemoNoteDto);

        Task RemoveAsync(int id);

        Task<int> GetCurrentReviewNumberAsync(int legalMemoId);

        Task CloseCurrentNotesAsync(int legalMemoId);
    }
}
