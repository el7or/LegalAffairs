using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Models;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Services
{
    public interface IMoamalatRaselService
    {
        Task<ReturnResult<QueryResultDto<MoamalaRaselListItemDto>>> GetAllAsync(MoamalatRaselQueryObject queryObject);

        Task<ReturnResult<MoamalaRaselDto>> GetAsync(int id);

        Task<ReturnResult<MoamalaRaselDto>> AddAsync(MoamalaRaselDto moamalaRaselDto);

        Task<ReturnResult<MoamalaRaselDto>> ReceiveAsync(int moamalaRaselId);

        Task<ReturnResult<bool>> RemoveAsync(int id);
    }
}