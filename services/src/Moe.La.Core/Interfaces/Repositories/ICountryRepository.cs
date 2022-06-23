using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Repositories
{
    public interface ICountryRepository
    {
        Task<QueryResultDto<CountryListItemDto>> GetAllAsync(QueryObject queryObject);

        Task<CountryDetailsDto> GetAsync(int id);

        Task AddAsync(CountryDto Country);

        Task EditAsync(CountryDto Country);

        Task RemoveAsync(int id);

        Task<bool> IsNameExistsAsync(CountryDto countryDto);
    }

}
