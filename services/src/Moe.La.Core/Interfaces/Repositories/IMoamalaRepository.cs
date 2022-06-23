using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Repositories
{
    public interface IMoamalaRepository
    {
        Task<QueryResultDto<MoamalaListItemDto>> GetAllAsync(MoamalatQueryObject queryObject);

        Task<MoamalaDetailsDto> GetAsync(int? id);

        Task AddAsync(MoamalaDto moamlaDto);

        Task EditAsync(MoamalaDto moamlaDto);

        Task RemoveAsync(int id);

        Task ChangeStatusAsync(MoamalaChangeStatusDto changeStatusDto);
        Task ReturnAsync(int id, string note);

        Task AddNotificationAsync(MoamalaNotificationDto moamalaNotificationDto);

        Task<MoamalaDetailsDto> UpdateWorkItemTypeAsync(MoamalaUpdateWorkItemType moamalaUpdateWorkItemType);

        Task<MoamalaDetailsDto> UpdateRelatedIdAsync(MoamalaUpdateRelatedId moamalaUpdateRelatedId);
    }
}
