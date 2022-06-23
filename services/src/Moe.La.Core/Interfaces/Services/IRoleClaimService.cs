using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Models;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Services
{
    public interface IRoleClaimService
    {
        Task<ReturnResult<QueryResultDto<RoleClaimListItemDto>>> GetAllAsync(QueryObject queryObject);
    }
}
