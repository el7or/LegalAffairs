using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Repositories
{
    public interface IDistrictRepository
    {
        Task<QueryResultDto<DistrictListItemDto>> GetAllAsync(DistrictQueryObject queryObject);

        Task<DistrictDto> GetAsync(int id);

        Task<DistrictDto> AddAsync(DistrictDto DistrictDto);

        Task EditAsync(DistrictDto saveDistrictDto);

        Task RemoveAsync(int id);

        Task<bool> IsNameExistsAsync(DistrictDto districtDto);
    }
}