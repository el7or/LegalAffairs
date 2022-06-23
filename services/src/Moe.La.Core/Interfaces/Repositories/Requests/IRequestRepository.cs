using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Enums;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Repositories
{
    public interface IRequestRepository
    {
        Task<QueryResultDto<RequestListItemDto>> GetAllAsync(RequestQueryObject queryObject);

        Task<RequestListItemDto> GetAsync(int id);

        Task ChangeRequestStatus(int requestId, RequestStatuses requestStatus);

        Task<RequestForPrintDto> GetForPrintAsync(int id);

        Task UpdateStatusAsync(int id, RequestStatuses status);

    }
}
