using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Models;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Services
{
    public interface IJobTitleService
    {
        Task<ReturnResult<QueryResultDto<JobTitleListItemDto>>> GetAllAsync(QueryObject queryObject);

        Task<ReturnResult<JobTitleListItemDto>> GetAsync(int id);

        Task<ReturnResult<JobTitleDto>> AddAsync(JobTitleDto model);

        Task<ReturnResult<JobTitleDto>> EditAsync(JobTitleDto model);

        Task<ReturnResult<bool>> RemoveAsync(int id);

        Task<ReturnResult<bool>> IsNameExistsAsync(string name);
    }
}
