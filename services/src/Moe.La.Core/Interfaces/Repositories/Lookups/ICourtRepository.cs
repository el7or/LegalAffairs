using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Repositories
{
    public interface ICourtRepository
    {
        Task<QueryResultDto<CourtListItemDto>> GetAllAsync(CourtQueryObject queryObject);

        Task<CourtListItemDto> GetAsync(int id);

        Task AddAsync(CourtDto courtDto);

        Task EditAsync(CourtDto saveCourtDto);

        Task RemoveAsync(int id);

        Task<bool> IsNameExistsAsync(CourtDto courtDto);
    }
}