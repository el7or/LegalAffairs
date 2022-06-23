using Moe.La.Core.Dtos;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Moe.La.ServiceInterface.Auth
{
    public interface IJwtFactory
    {
        Task<string> GenerateEncodedToken(
            string userName,
            string firstName,
            string jobTitle,
            string pictureUrl,
            ClaimsIdentity identity,
            IList<string> roles,
            IList<string> permissions,
            IList<string> departments,
            string branch
            );
        ClaimsIdentity GenerateClaimsIdentity(string userName, string id);
        public RefreshTokenDto GenerateRefreshToken();

    }
}
