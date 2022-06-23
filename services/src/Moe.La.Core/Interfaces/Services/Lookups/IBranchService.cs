using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Services
{
    public interface IBranchService
    {
        Task<ReturnResult<IList<BranchListItemDto>>> GetAllAsync();

        Task<ReturnResult<QueryResultDto<BranchListItemDto>>> GetAllAsync(BranchQueryObject queryObject);

        Task<ReturnResult<BranchDto>> GetAsync(int id);

        /// <summary>
        /// Get all departments related to a given branch.
        /// </summary>
        /// <param name="id">Branch ID.</param>
        /// <returns></returns>
        Task<ReturnResult<IList<DepartmentListItemDto>>> GetDepartmentsAsync(int id);

        Task<ReturnResult<BranchListItemDto>> GetByNameAsync(string name);

        Task<ReturnResult<BranchDto>> AddAsync(BranchDto model);

        Task<ReturnResult<BranchDto>> EditAsync(BranchDto model);

        Task<ReturnResult<bool>> RemoveAsync(int id);

        Task<ReturnResult<bool>> IsNameExistsAsync(BranchDto model);

    }
}
