using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using YC5_API_IO.Dto;
using YC5_API_IO.Interfaces;

namespace YC5_API_IO.Services
{
    public class JWTService : IJwtInterface
    {
        private readonly IConfiguration _configuration;
        private readonly byte[] _secretKey;

        public JWTService(IConfiguration configuration)
        {
            _configuration = configuration;
            _secretKey = Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"] ?? throw new InvalidOperationException("JwtSettings:SecretKey not configured."));
        }

        public JwtTokenDto GenerateJwtTokens(string userId, string roleName, string userName)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.Role, roleName)
            };

            var accessTokenExpiresMinutes = double.Parse(_configuration["JwtSettings:AccessTokenExpirationMinutes"] ?? "15");
            var refreshTokenExpiresMinutes = double.Parse(_configuration["JwtSettings:RefreshTokenExpirationMinutes"] ?? "10080"); // 7 days

            var accessToken = GenerateToken(claims, accessTokenExpiresMinutes);
            var refreshToken = GenerateToken(claims, refreshTokenExpiresMinutes); // Refresh token can also carry claims or be a simple string

            return new JwtTokenDto
            {
                AccessToken = accessToken,
                AccessTokenExpires = DateTime.UtcNow.AddMinutes(accessTokenExpiresMinutes),
                RefreshToken = refreshToken,
                RefreshTokenExpires = DateTime.UtcNow.AddMinutes(refreshTokenExpiresMinutes)
            };
        }

        private string GenerateToken(Claim[] claims, double expirationMinutes)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(expirationMinutes),
                Issuer = _configuration["JwtSettings:Issuer"],
                Audience = _configuration["JwtSettings:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_secretKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, // We don't care about the audience for expired token validation
                ValidateIssuer = false, // We don't care about the issuer for expired token validation
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(_secretKey),
                ValidateLifetime = false // Here we are saying that we don't care about the token's expiration date
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;

            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }

        public JwtTokenDto RefreshAccessToken(string accessToken, string refreshToken)
        {
            var principal = GetPrincipalFromExpiredToken(accessToken);
            if (principal == null)
            {
                throw new SecurityTokenException("Invalid access token");
            }

            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var roleName = principal.FindFirst(ClaimTypes.Role)?.Value;
            var userName = principal.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(roleName) || string.IsNullOrEmpty(userName))
            {
                throw new SecurityTokenException("Invalid claims in access token");
            }

            // Validate refresh token
            var refreshTokenPrincipal = ValidateRefreshToken(refreshToken);
            if (refreshTokenPrincipal == null)
            {
                throw new SecurityTokenException("Invalid refresh token");
            }
            
            // Here, you would typically also verify the refresh token against a stored one in your database
            // to ensure it hasn't been revoked and matches the user. For this example, we skip the database check.

            return GenerateJwtTokens(userId, roleName, userName);
        }

        private ClaimsPrincipal? ValidateRefreshToken(string refreshToken)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(_secretKey),
                ValidateLifetime = true // We DO care about the refresh token's expiration date
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var principal = tokenHandler.ValidateToken(refreshToken, tokenValidationParameters, out SecurityToken securityToken);
                var jwtSecurityToken = securityToken as JwtSecurityToken;

                if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new SecurityTokenException("Invalid refresh token algorithm");
                }
                return principal;
            }
            catch (Exception)
            {
                return null;
            }
        }

    }
}