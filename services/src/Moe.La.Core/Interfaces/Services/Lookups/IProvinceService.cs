using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Models;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Services
{
    public interface IProvinceService
    {
        Task<ReturnResult<QueryResultDto<ProvinceListItemDto>>> GetAllAsync(ProvinceQueryObject queryObject);

        Task<ReturnResult<ProvinceListItemDto>> GetAsync(int id);

        Task<ReturnResult<ProvinceDto>> AddAsync(ProvinceDto model);

        Task<ReturnResult<ProvinceDto>> EditAsync(ProvinceDto model);

        Task<ReturnResult<bool>> RemoveAsync(int id);

        Task<ReturnResult<bool>> IsNameExistsAsync(string name);

    }
}