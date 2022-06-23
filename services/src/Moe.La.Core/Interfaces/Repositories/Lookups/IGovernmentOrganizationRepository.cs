using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Repositories
{
    public interface IGovernmentOrganizationRepository
    {
        Task<QueryResultDto<GovernmentOrganizationListItemDto>> GetAllAsync(QueryObject queryObject);

        Task<GovernmentOrganizationListItemDto> GetAsync(int id);

        Task<GovernmentOrganizationDto> AddAsync(GovernmentOrganizationDto GovernmentOrganizationDto);

        Task EditAsync(GovernmentOrganizationDto saveGovernmentOrganizationDto);

        Task RemoveAsync(int id);

        Task<bool> IsNameExistsAsync(string name);
    }
}