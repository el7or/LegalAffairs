using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Models;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Services
{
    public interface IFirstSubCategoryService
    {
        Task<ReturnResult<QueryResultDto<FirstSubCategoryListItemDto>>> GetAllAsync(FirstSubCategoriesQueryObject queryObject);

        Task<ReturnResult<FirstSubCategoryListItemDto>> GetAsync(int id);

        Task<ReturnResult<FirstSubCategoryDto>> AddAsync(FirstSubCategoryDto firstSubCategoryDto);

        Task<ReturnResult<FirstSubCategoryDto>> EditAsync(FirstSubCategoryDto firstSubCategoryDto);

        Task<ReturnResult<bool>> RemoveAsync(int id);

        Task<ReturnResult<bool>> IsNameExistsAsync(FirstSubCategoryDto firstSubCategoryDto);
    }
}
