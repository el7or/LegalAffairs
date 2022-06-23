using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Models;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Services
{
    public interface ICountryService
    {
        Task<ReturnResult<QueryResultDto<CountryListItemDto>>> GetAllAsync(QueryObject queryObject);

        Task<ReturnResult<CountryDetailsDto>> GetAsync(int id);

        Task<ReturnResult<CountryDto>> AddAsync(CountryDto model);

        Task<ReturnResult<CountryDto>> EditAsync(CountryDto model);

        Task<ReturnResult<bool>> RemoveAsync(int id);
    }
}
