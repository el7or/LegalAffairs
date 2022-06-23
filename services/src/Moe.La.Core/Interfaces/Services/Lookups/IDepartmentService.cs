using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Models;
using System;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Services
{
    public interface IDepartmentService
    {
        Task<ReturnResult<QueryResultDto<DepartmentListItemDto>>> GetAllAsync(QueryObject queryObject);

        Task<ReturnResult<DepartmentListItemDto>> GetAsync(int id);

        Task<ReturnResult<DepartmentDto>> AddAsync(DepartmentDto model);

        Task<ReturnResult<DepartmentDto>> EditAsync(DepartmentDto model);

        Task<ReturnResult<bool>> RemoveAsync(int id, Guid userId);

        Task<ReturnResult<bool>> IsNameExistsAsync(string name);
    }
}
