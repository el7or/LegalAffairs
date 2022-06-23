using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Repositories
{
    public interface IPartyRepository
    {
        Task<QueryResultDto<PartyListItemDto>> GetAllAsync(PartyQueryObject queryObject);

        Task<PartyDetailsDto> GetAsync(int id);

        Task AddAsync(PartyDto Party);

        Task EditAsync(PartyDto Party);

        Task RemoveAsync(int id);

        Task<bool> IsPartyExist(PartyDto partyDto);
    }
}
