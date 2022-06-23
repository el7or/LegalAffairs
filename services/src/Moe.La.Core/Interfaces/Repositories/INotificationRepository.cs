using Moe.La.Core.Dtos;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Repositories
{
    public interface INotificationRepository
    {
        //Task<QueryResultDto<NotificationListItemDto>> GetAllAsync(NotificationSystemQueryObject queryObject);

        //Task<NotificationDetailsDto> GetAsync(int id);

        Task AddAsync(NotificationDto Notification);

        //Task EditAsync(NotificationDto Notification);

        //Task ReadAsync(int id);

        //Task RemoveAsync(int id);
    }
}
