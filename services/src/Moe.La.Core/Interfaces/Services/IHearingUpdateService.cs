using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Models;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Services
{
    public interface IHearingUpdateService
    {
        Task<ReturnResult<QueryResultDto<HearingUpdateListItemDto>>> GetAllAsync(HearingUpdateQueryObject queryObject);

        Task<ReturnResult<HearingUpdateDetailsDto>> GetAsync(int id);

        Task<ReturnResult<HearingUpdateDto>> AddAsync(HearingUpdateDto model);

        Task<ReturnResult<HearingUpdateDto>> EditAsync(HearingUpdateDto model);

    }
}