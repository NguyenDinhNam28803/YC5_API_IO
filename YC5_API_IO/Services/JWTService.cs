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
    }
}