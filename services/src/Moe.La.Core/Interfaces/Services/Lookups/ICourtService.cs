using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Models;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Services
{
    public interface ICourtService
    {
        Task<ReturnResult<QueryResultDto<CourtListItemDto>>> GetAllAsync(CourtQueryObject queryObject);

        Task<ReturnResult<CourtListItemDto>> GetAsync(int id);

        Task<ReturnResult<CourtDto>> AddAsync(CourtDto model);

        Task<ReturnResult<CourtDto>> EditAsync(CourtDto model);

        Task<ReturnResult<int>> RemoveAsync(int id);

        Task<ReturnResult<bool>> IsNameExistsAsync(CourtDto courtDto);
    }
}