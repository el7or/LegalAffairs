using Moe.La.Core.Dtos;
using Moe.La.Core.Models;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Services
{
    public interface ICaseSupportingDocumentRequestHistoryService
    {
        Task AddAsync(int id);
        Task<ReturnResult<CaseSupportingDocumentRequestHistoryListItemDto>> GetAsync(int id);
    }
}