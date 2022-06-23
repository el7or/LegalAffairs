using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Repositories
{
    public interface INotificationEmailRepository
    {
        Task<QueryResultDto<NotificationEmailListItemDto>> GetAllAsync(NotificationEmailQueryObject queryObject);

        Task SentSuccessufullyAsync(NotificationEmailListItemDto notificationEmail);
    }
}
