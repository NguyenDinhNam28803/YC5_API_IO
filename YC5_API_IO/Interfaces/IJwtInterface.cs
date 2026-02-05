using System.Security.Claims;
using YC5_API_IO.Dto; // Assuming a DTO for token claims or user info

namespace YC5_API_IO.Interfaces
{
    public interface IJwtInterface
    {
        JwtTokenDto GenerateJwtTokens(string userId, string roleName, string userName);
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    }
}
