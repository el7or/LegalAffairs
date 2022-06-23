using Moe.La.Core.Dtos;
using Moe.La.Core.Models;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Services
{
    public interface IAuthService
    {
        Task<ReturnResult<object>> Login(CredentialsDto credentials, string rule_name = null);
        Task<ReturnResult<object>> RefreshToken(string refreshToken);
        Task<ReturnResult<bool>> RevokeToken(string token);
    }
}