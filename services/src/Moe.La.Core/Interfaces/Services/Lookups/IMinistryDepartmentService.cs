using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Models;
using System;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Services
{
    public interface IMinistryDepartmentService
    {
        Task<ReturnResult<QueryResultDto<MinistryDepartmentListItemDto>>> GetAllAsync(MinistryDepartmentQueryObject queryObject);

        Task<ReturnResult<MinistryDepartmentListItemDto>> GetAsync(int id);

        Task<ReturnResult<MinistryDepartmentDto>> AddAsync(MinistryDepartmentDto model);

        Task<ReturnResult<MinistryDepartmentDto>> EditAsync(MinistryDepartmentDto model);

        Task<ReturnResult<bool>> RemoveAsync(int id, Guid userId);

        Task<ReturnResult<int>> GetDepartmentIdAsync(string name);

        Task<ReturnResult<bool>> IsNameExistsAsync(MinistryDepartmentDto ministryDepartmentDto);
    }
}
