using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Models;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Services
{
    public interface IPartyService
    {
        Task<ReturnResult<QueryResultDto<PartyListItemDto>>> GetAllAsync(PartyQueryObject queryObject);

        Task<ReturnResult<PartyDetailsDto>> GetAsync(int id);

        Task<ReturnResult<PartyDto>> AddAsync(PartyDto model);

        Task<ReturnResult<PartyDto>> EditAsync(PartyDto model);

        Task<ReturnResult<bool>> RemoveAsync(int id);

        Task<ReturnResult<bool>> IsPartyExist(PartyDto partyDto);
    }
}
