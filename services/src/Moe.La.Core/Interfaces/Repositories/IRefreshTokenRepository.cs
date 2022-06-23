using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task<ICollection<RefreshToken>> GetAllAsync();

        Task<RefreshToken> GetAsync(int Id);

        Task<RefreshTokenDto> AddAsync(RefreshTokenDto refreshTokenDto);

        Task EditAsync(RefreshTokenDto refreshTokenDto);

        Task RemoveAsync(int id);

    }
}