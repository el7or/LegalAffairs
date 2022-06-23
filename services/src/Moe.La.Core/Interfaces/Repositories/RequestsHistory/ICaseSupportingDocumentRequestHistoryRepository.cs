using Moe.La.Core.Dtos;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Repositories
{
    public interface ICaseSupportingDocumentRequestHistoryRepository
    {
        Task AddAsync(int id);
        Task<CaseSupportingDocumentRequestHistoryListItemDto> GetAsync(int id);

    }
}
