using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Repositories
{
    public interface INotificationSMSRepository
    {
        Task<QueryResultDto<NotificationSMSListItemDto>> GetAllAsync(NotificationSMSQueryObject queryObject);

        Task SentSuccessufullyAsync(NotificationSMSListItemDto sms);
    }
}
