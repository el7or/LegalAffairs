using Moe.La.Core.Dtos;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Repositories
{
    public interface IConsultationSupportingDocumentRequestHistoryRepository
    {
        Task AddAsync(int id);
        Task<ConsultationSupportingDocumentHistoryListItemDto> GetAsync(int id);

    }
}
