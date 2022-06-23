using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Repositories
{
    public interface ICityRepository
    {
        Task<QueryResultDto<CityListItemDto>> GetAllAsync(CityQueryObject queryObject);

        Task<CityListItemDto> GetAsync(int id);

        Task<CityListItemDto> GetByNameAsync(string name);

        Task AddAsync(CityDto cityDto);

        Task EditAsync(CityDto saveCityDto);

        Task RemoveAsync(int id);

        Task<bool> IsNameExistsAsync(CityDto cityDto);
    }
}