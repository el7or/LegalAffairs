using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Repositories
{
    public interface IWorkItemTypeRepository
    {
        Task<QueryResultDto<WorkItemTypeListItemDto>> GetAllAsync(WorkItemTypeQueryObject queryObject);

        Task<WorkItemTypeDto> GetAsync(int id);

        Task<int> GetByNameAsync(string name);

        //Task AddAsync(WorkItemTypeDto workItemType);

        Task EditAsync(WorkItemTypeDto workItemTypeDto);

        Task RemoveAsync(int id);

        Task<bool> IsNameExistsAsync(WorkItemTypeDto workItemTypeDto);
    }
}
