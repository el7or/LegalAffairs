using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Repositories
{
    public interface IRoleClaimRepository
    {
        Task<QueryResultDto<RoleClaimListItemDto>> GetAllAsync(QueryObject queryObject);
    }
}
