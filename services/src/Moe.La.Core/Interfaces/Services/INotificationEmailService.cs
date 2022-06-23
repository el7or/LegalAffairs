using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Models;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Services
{
    public interface INotificationEmailService
    {
        Task<ReturnResult<QueryResultDto<NotificationEmailListItemDto>>> GetAllAsync(NotificationEmailQueryObject queryObject);

        Task SendEmails();

        //Task<ReturnResult<bool>> SendAsync(int notificationId, Guid userId);

    }
}
