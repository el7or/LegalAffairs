using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Models;
using System;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Services
{
    public interface IRoleService
    {
        Task<ReturnResult<QueryResultDto<RoleListItemDto>>> GetAllAsync(QueryObject queryObject);

        Task<ReturnResult<RoleListItemDto>> GetAsync(Guid id);

        Task<ReturnResult<RoleDto>> AddAsync(RoleDto model);

        Task<ReturnResult<RoleDto>> EditAsync(RoleDto model);

        Task<ReturnResult<bool>> RemoveAsync(Guid id);
    }
}
