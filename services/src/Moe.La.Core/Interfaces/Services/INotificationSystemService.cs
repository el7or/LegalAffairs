using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Models;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Services
{
    public interface INotificationSystemService
    {
        Task<ReturnResult<QueryResultDto<NotificationSystemListItemDto>>> GetAllAsync(NotificationSystemQueryObject queryObject);

        Task<ReturnResult<NotificationSystemDetailsDto>> GetAsync(int id);

        Task<ReturnResult<bool>> ReadAsync(int id);

    }
}
