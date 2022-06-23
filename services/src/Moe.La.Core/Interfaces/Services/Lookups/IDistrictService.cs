using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Models;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Services
{
    public interface IDistrictService
    {
        Task<ReturnResult<QueryResultDto<DistrictListItemDto>>> GetAllAsync(DistrictQueryObject queryObject);

        Task<ReturnResult<DistrictDto>> GetAsync(int id);

        Task<ReturnResult<DistrictDto>> AddAsync(DistrictDto model);

        Task<ReturnResult<DistrictDto>> EditAsync(DistrictDto model);

        Task<ReturnResult<bool>> RemoveAsync(int id);

        Task<ReturnResult<bool>> IsNameExistsAsync(DistrictDto districtDto);

    }
}
