using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Repositories
{
    public interface IHearingUpdateRepository
    {
        Task<QueryResultDto<HearingUpdateListItemDto>> GetAllAsync(HearingUpdateQueryObject queryObject);

        Task<HearingUpdateDetailsDto> GetAsync(int id);

        Task AddAsync(HearingUpdateDto hearingDto);
        Task EditAsync(HearingUpdateDto hearingDto);

    }
}