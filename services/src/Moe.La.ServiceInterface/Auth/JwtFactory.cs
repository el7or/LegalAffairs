using Microsoft.Extensions.Options;
using Moe.La.Core.Dtos;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Threading.Tasks;
using static Moe.La.ServiceInterface.Auth.Constants.Strings;

namespace Moe.La.ServiceInterface.Auth
{
    public class JwtFactory : IJwtFactory
    {
        private readonly JwtIssuerOptions jwtIssuerOptions;

        public JwtFactory(IOptions<JwtIssuerOptions> jwtOptions)
        {
            jwtIssuerOptions = jwtOptions.Value;
            ThrowIfInvalidOptions(jwtIssuerOptions);
        }

        public async Task<string> GenerateEncodedToken(
            string userName,
            string firstName,
            string jobTitle,
            string pictureUrl,
            ClaimsIdentity identity,
            IList<string> roles,
            IList<string> permissions,
            IList<string> departments,
            string branch)
        {
            ////Add claim (jwtIssuerOptions + user data)
            var claims = new List<Claim> {
                 new Claim(JwtRegisteredClaimNames.Sub, userName),
                 new Claim(JwtRegisteredClaimNames.GivenName, firstName),
                 new Claim("jobTitle", jobTitle),
                 new Claim("pic", pictureUrl),
                 new Claim(JwtRegisteredClaimNames.Jti, await jwtIssuerOptions.JtiGenerator()),
                 new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(jwtIssuerOptions.IssuedAt).ToString(), ClaimValueTypes.Integer64),
                 identity.FindFirst("id")};

            foreach (var role in roles)
            {
                claims.Add(new Claim(MyJwtClaimIdentifiers.Roles, role));
            }

            foreach (var permission in permissions)
            {
                claims.Add(new Claim(MyJwtClaimIdentifiers.Permission, permission));
            }

            foreach (var department in departments)
            {
                claims.Add(new Claim(MyJwtClaimIdentifiers.Department, department));
            }

            claims.Add(new Claim(MyJwtClaimIdentifiers.Branch, branch));

            // Create the JWT security token and encode it.
            var jwt = new JwtSecurityToken(
                issuer: jwtIssuerOptions.Issuer,
                audience: jwtIssuerOptions.Audience,
                claims: claims,
                notBefore: jwtIssuerOptions.NotBefore,
                expires: jwtIssuerOptions.Expiration,
                signingCredentials: jwtIssuerOptions.SigningCredentials);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }

        public ClaimsIdentity GenerateClaimsIdentity(string userName, string id)
        {
            return new ClaimsIdentity(new GenericIdentity(userName, "Token"), new[]
            {
                new Claim("id", id),
                new Claim(MyJwtClaimIdentifiers.Roles, MyJwtClaims.ApiAccess)
            });
        }

        /// <returns>Date converted to seconds since Unix epoch (Jan 1, 1970, midnight UTC).</returns>
        private static long ToUnixEpochDate(DateTime date)
          => (long)Math.Round((date.ToUniversalTime() -
                               new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
                              .TotalSeconds);

        private static void ThrowIfInvalidOptions(JwtIssuerOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            if (options.ValidFor <= TimeSpan.Zero)
            {
                throw new ArgumentException("Must be a non-zero TimeSpan.", nameof(JwtIssuerOptions.ValidFor));
            }

            if (options.SigningCredentials == null)
            {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.SigningCredentials));
            }

            if (options.JtiGenerator == null)
            {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.JtiGenerator));
            }
        }

        public RefreshTokenDto GenerateRefreshToken()
        {
            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[64];
                rngCryptoServiceProvider.GetBytes(randomBytes);
                return new RefreshTokenDto
                {
                    Token = Convert.ToBase64String(randomBytes),
                    Expires = DateTime.UtcNow.AddDays(1),
                };
            }
        }


    }
}
