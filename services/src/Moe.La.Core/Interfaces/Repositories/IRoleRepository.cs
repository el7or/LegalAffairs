using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using System;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Repositories
{
    public interface IRoleRepository
    {
        Task<QueryResultDto<RoleListItemDto>> GetAllAsync(QueryObject queryObject);

        Task<RoleListItemDto> GetAsync(Guid id);

        Task AddAsync(RoleDto roleDto);

        Task EditAsync(RoleDto roleDto);

        Task RemoveAsync(Guid id);
    }
}