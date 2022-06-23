using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using System;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Repositories
{
    public interface IDepartmentRepository
    {
        Task<QueryResultDto<DepartmentListItemDto>> GetAllAsync(QueryObject queryObject);

        Task<DepartmentListItemDto> GetAsync(int id);

        Task EditAsync(DepartmentDto internalDepartment);

        Task RemoveAsync(int id, Guid userId);

        Task AddAsync(DepartmentDto internalDepartment);

        Task<bool> IsNameExistsAsync(string name);
    }
}
