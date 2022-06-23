using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Repositories
{
    public interface IBranchRepository
    {
        Task<IList<BranchListItemDto>> GetAllAsync();

        Task<QueryResultDto<BranchListItemDto>> GetAllAsync(BranchQueryObject queryObject);

        Task<BranchDto> GetAsync(int id);

        /// <summary>
        /// Get all departments related to a given branch.
        /// </summary>
        /// <param name="id">Branch ID.</param>
        /// <returns></returns>
        Task<IList<DepartmentListItemDto>> GetDepartmentsAsync(int id);

        Task<BranchListItemDto> GetByNameAsync(string name);

        Task AddAsync(BranchDto branchDto);

        Task EditAsync(BranchDto branchDto);

        Task RemoveAsync(int id);

        Task<bool> IsNameExistsAsync(BranchDto branchDto);
    }
}