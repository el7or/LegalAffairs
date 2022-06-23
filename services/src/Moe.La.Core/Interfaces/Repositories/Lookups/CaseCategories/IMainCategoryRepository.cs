using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Repositories
{
    public interface IMainCategoryRepository
    {
        Task<MainCategoryDto> AddAsync(MainCategoryDto mainCategoryDto);

        Task EditAsync(MainCategoryDto mainCategoryDto);

        Task<QueryResultDto<MainCategoryListItemDto>> GetAllAsync(MainCategoryQueryObject queryObject);

        Task<MainCategoryListItemDto> GetAsync(int id);

        Task RemoveAsync(int id);

        Task<bool> IsNameExistsAsync(MainCategoryDto mainCategoryDto);
    }
}