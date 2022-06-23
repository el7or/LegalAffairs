using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Repositories
{
    public interface IJobTitleRepository
    {
        Task<QueryResultDto<JobTitleListItemDto>> GetAllAsync(QueryObject queryObject);

        Task<JobTitleListItemDto> GetAsync(int id);

        Task AddAsync(JobTitleDto JobTitle);

        Task EditAsync(JobTitleDto JobTitle);

        Task RemoveAsync(int id);

        Task<bool> IsNameExistsAsync(string name);
    }
}
