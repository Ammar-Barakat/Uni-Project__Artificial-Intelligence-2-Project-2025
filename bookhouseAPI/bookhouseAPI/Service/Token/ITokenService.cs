using bookhouseAPI.Models;
using System.IdentityModel.Tokens.Jwt;

namespace bookhouseAPI.Service.Token
{
    public interface ITokenService
    {
        Task<JwtSecurityToken> CreateTokenAsync(ApplicationUser user);
    }
}
