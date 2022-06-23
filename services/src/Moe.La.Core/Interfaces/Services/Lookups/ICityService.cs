using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Models;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Services
{
    public interface ICityService
    {
        Task<ReturnResult<QueryResultDto<CityListItemDto>>> GetAllAsync(CityQueryObject queryObject);

        Task<ReturnResult<CityListItemDto>> GetAsync(int id);

        Task<ReturnResult<CityDto>> AddAsync(CityDto model);

        Task<ReturnResult<CityDto>> EditAsync(CityDto model);

        Task<ReturnResult<bool>> RemoveAsync(int id);

        Task<ReturnResult<bool>> IsNameExistsAsync(CityDto cityDto);
    }
}
