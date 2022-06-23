using Moe.La.Core.Dtos;
using Moe.La.Core.Models;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Services
{
    public interface INotificationService
    {
        //Task<ReturnResult<QueryResultDto<NotificationDto>>> GetAllAsync(NotificationSystemQueryObject queryObject);

        //Task<ReturnResult<NotificationDto>> GetAsync(int id);

        Task<ReturnResult<NotificationDto>> AddAsync(NotificationDto model);

        //Task<ReturnResult<NotificationDto>> EditAsync(NotificationDto model);

        //Task<ReturnResult<bool>> ReadAsync(int id);

        //Task<ReturnResult<bool>> RemoveAsync(int id);
    }
}
