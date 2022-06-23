using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Repositories
{
    public interface IMoamalaRaselRepository
    {
        Task<QueryResultDto<MoamalaRaselListItemDto>> GetAllAsync(MoamalatRaselQueryObject queryObject);

        Task<MoamalaRaselDto> GetAsync(int id);

        Task AddAsync(MoamalaRaselDto moamalaRasel);

        Task<MoamalaRaselDto> ReceiveMoamalaAsync(int id);

        Task RemoveAsync(int id);
    }
}