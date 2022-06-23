using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Models;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Services
{
    public interface ISubWorkItemTypeService
    {
        Task<ReturnResult<QueryResultDto<SubWorkItemTypeListItemDto>>> GetAllAsync(SubWorkItemTypeQueryObject queryObject);

        Task<ReturnResult<SubWorkItemTypeDto>> GetAsync(int id);

        Task<ReturnResult<SubWorkItemTypeDto>> AddAsync(SubWorkItemTypeDto model);

        Task<ReturnResult<SubWorkItemTypeDto>> EditAsync(SubWorkItemTypeDto model);

        Task<ReturnResult<bool>> RemoveAsync(int id);

        Task<ReturnResult<bool>> IsNameExistsAsync(SubWorkItemTypeDto subWorkItemTypeDto);
    }
}
