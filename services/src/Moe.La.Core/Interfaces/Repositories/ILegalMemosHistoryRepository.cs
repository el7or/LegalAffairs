using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Repositories
{
    public interface ILegalMemosHistoryRepository
    {
        Task<QueryResultDto<LegalMemosHistoryListItemDto>> GetAllAsync(LegalMemoQueryObject queryObject);

        Task AddAsync(LegalMemosHistoryDto legalMemoDto);
    }
}
