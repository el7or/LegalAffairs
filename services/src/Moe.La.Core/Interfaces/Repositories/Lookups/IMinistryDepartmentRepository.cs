using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using System;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Repositories
{
    public interface IMinistryDepartmentRepository
    {
        Task<QueryResultDto<MinistryDepartmentListItemDto>> GetAllAsync(MinistryDepartmentQueryObject queryObject);

        Task<MinistryDepartmentListItemDto> GetAsync(int id);

        Task EditAsync(MinistryDepartmentDto ministryDepartmentDto);

        Task RemoveAsync(int id, Guid userId);

        Task AddAsync(MinistryDepartmentDto ministryDepartmentDto);

        Task<int> GetDepartmentIdAsync(string Name);

        Task<bool> IsNameExistsAsync(MinistryDepartmentDto ministryDepartmentDto);
    }
}
