using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Models;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Services
{
    public interface IMainCategoryService
    {
        Task<ReturnResult<QueryResultDto<MainCategoryListItemDto>>> GetAllAsync(MainCategoryQueryObject queryObject);

        Task<ReturnResult<MainCategoryListItemDto>> GetAsync(int id);

        Task<ReturnResult<MainCategoryDto>> AddAsync(MainCategoryDto mainCategoryDto);

        Task<ReturnResult<MainCategoryDto>> EditAsync(MainCategoryDto mainCategoryDto);

        Task<ReturnResult<bool>> RemoveAsync(int id);

        Task<ReturnResult<bool>> IsNameExistsAsync(MainCategoryDto mainCategoryDto);
    }
}