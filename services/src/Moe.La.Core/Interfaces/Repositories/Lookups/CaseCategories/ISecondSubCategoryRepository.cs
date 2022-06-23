using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Repositories
{
    public interface ISecondSubCategoryRepository
    {
        Task<QueryResultDto<SecondSubCategoryListItemDto>> GetAllAsync(SecondSubCategoryQueryObject queryObject);

        Task<SecondSubCategoryDto> GetAsync(int id);

        Task<SecondSubCategoryDto> AddAsync(SecondSubCategoryDto secondSubCategoryDto);

        Task EditAsync(SecondSubCategoryDto secondSubCategoryDto);

        Task RemoveAsync(int id);

        Task<bool> IsNameExistsAsync(SecondSubCategoryDto secondSubCategoryDto);

        Task ChangeCategoryActivity(int SecondSubCategoryId, bool IsActive);

    }
}