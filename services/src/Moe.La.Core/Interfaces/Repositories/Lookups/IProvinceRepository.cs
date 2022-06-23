using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Repositories
{
    public interface IProvinceRepository
    {
        Task AddAsync(ProvinceDto provinceDto);

        Task EditAsync(ProvinceDto provinceDto);

        Task<QueryResultDto<ProvinceListItemDto>> GetAllAsync(ProvinceQueryObject queryObject);

        Task<ProvinceListItemDto> GetAsync(int id);

        Task RemoveAsync(int id);

        Task<bool> IsNameExistsAsync(string name);
    }
}