using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Repositories
{
    public interface INotificationSystemRepository
    {
        Task<QueryResultDto<NotificationSystemListItemDto>> GetAllAsync(NotificationSystemQueryObject queryObject);

        Task<NotificationSystemDetailsDto> GetAsync(int id);

        Task ReadAsync(int id);
    }
}
