using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Repositories
{
    public interface IFirstSubCategoryRepository
    {
        Task<QueryResultDto<FirstSubCategoryListItemDto>> GetAllAsync(FirstSubCategoriesQueryObject queryObject);

        Task<FirstSubCategoryListItemDto> GetAsync(int id);

        Task<FirstSubCategoryListItemDto> GetByNameAsync(string name);

        Task<FirstSubCategoryDto> AddAsync(FirstSubCategoryDto firstSubCategoryDto);

        Task EditAsync(FirstSubCategoryDto firstSubCategoryDto);

        Task RemoveAsync(int id);

        Task<bool> IsNameExistsAsync(FirstSubCategoryDto firstSubCategoryDto);
    }
}