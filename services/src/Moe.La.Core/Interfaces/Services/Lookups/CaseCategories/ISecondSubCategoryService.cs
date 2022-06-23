using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Models;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Services
{
    public interface ISecondSubCategoryService
    {
        Task<ReturnResult<QueryResultDto<SecondSubCategoryListItemDto>>> GetAllAsync(SecondSubCategoryQueryObject queryObject);

        Task<ReturnResult<SecondSubCategoryDto>> GetAsync(int id);

        Task<ReturnResult<SecondSubCategoryDto>> AddAsync(SecondSubCategoryDto secondSubCategoryDto);

        Task<ReturnResult<SecondSubCategoryDto>> EditAsync(SecondSubCategoryDto secondSubCategoryDto);

        Task<ReturnResult<bool>> RemoveAsync(int id);

        Task<ReturnResult<bool>> IsNameExistsAsync(SecondSubCategoryDto secondSubCategoryDto);

        Task<ReturnResult<bool>> ChangeCatergoryActivityAsync(int secondSubCategoryId, bool isActive);

    }
}
