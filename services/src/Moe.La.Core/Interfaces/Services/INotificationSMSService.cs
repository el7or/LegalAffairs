using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Models;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Services
{
    public interface INotificationSMSService
    {
        Task<ReturnResult<QueryResultDto<NotificationSMSListItemDto>>> GetAllAsync(NotificationSMSQueryObject queryObject);

        Task SendSMSs();

    }
}
