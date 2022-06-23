using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Models;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Services
{
    public interface IWorkItemTypeService
    {
        Task<ReturnResult<QueryResultDto<WorkItemTypeListItemDto>>> GetAllAsync(WorkItemTypeQueryObject queryObject);

        Task<ReturnResult<WorkItemTypeDto>> GetAsync(int id);

        Task<ReturnResult<int>> GetByNameAsync(string name);

        //Task<ReturnResult<WorkItemTypeDto>> AddAsync(WorkItemTypeDto model);

        Task<ReturnResult<WorkItemTypeDto>> EditAsync(WorkItemTypeDto model);

        Task<ReturnResult<bool>> RemoveAsync(int id);

        Task<ReturnResult<bool>> IsNameExistsAsync(WorkItemTypeDto workItemTypeDto);
    }
}
