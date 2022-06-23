using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Repositories
{
    public interface ISubWorkItemTypeRepository
    {
        Task<QueryResultDto<SubWorkItemTypeListItemDto>> GetAllAsync(SubWorkItemTypeQueryObject queryObject);

        Task<SubWorkItemTypeDto> GetAsync(int id);

        Task AddAsync(SubWorkItemTypeDto subWorkItemTypeDto);

        Task EditAsync(SubWorkItemTypeDto SubWorkItemTypeDto);

        Task RemoveAsync(int id);

        Task<bool> IsNameExistsAsync(SubWorkItemTypeDto subWorkItemTypeDto);
    }
}
