using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Models;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Services
{
    public interface IGovernmentOrganizationService
    {
        Task<ReturnResult<QueryResultDto<GovernmentOrganizationListItemDto>>> GetAllAsync(QueryObject queryObject);

        Task<ReturnResult<GovernmentOrganizationListItemDto>> GetAsync(int id);

        Task<ReturnResult<GovernmentOrganizationDto>> AddAsync(GovernmentOrganizationDto model);

        Task<ReturnResult<GovernmentOrganizationDto>> EditAsync(GovernmentOrganizationDto model);

        Task<ReturnResult<bool>> RemoveAsync(int id);

        Task<ReturnResult<bool>> IsNameExistsAsync(string name);
    }
}
