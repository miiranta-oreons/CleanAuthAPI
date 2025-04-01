using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.Users.Commands.Common;
using Application.Users.Commands.RefreshTokens;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services.TokenService
{
    public class TokenService(AppDbContext context, UserManager<User> userManager, IConfiguration configuration) : ITokenService
    {

        private async Task<string> CreateTokenAsync(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var userRoles = await userManager.GetRolesAsync(user);
            foreach (var userRole in userRoles)
            {
               claims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("TokenSettings:Token")));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: configuration.GetValue<string>("TokenSettings:Issuer"),
                audience: configuration.GetValue<string>("TokenSettings:Audience"),
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

        private string CreateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private async Task<string> GenerateAndSaveRefreshTokenAsync(User user)
        {
            var refreshToken = CreateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiration = DateTime.UtcNow.AddDays(7);
            await context.SaveChangesAsync();
            return refreshToken;
        }

        private async Task<User?> ValidateRefreshTokenAsync(Guid userId, string refreshToken)
        {
            var user = await context.Users.FindAsync(userId);
            if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiration < DateTime.UtcNow)
            {
                return null;
            }
            return user;
        }

        public async Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokensCommand request)
        {
            var user = await ValidateRefreshTokenAsync(request.UserId, request.RefreshToken);
            if (user is null)
            {
                return null;
            }

            return await CreateTokenResponse(user);
        }

        public async Task<TokenResponseDto> CreateTokenResponse(User user)
        {
            return new TokenResponseDto()
            {
                AccessToken = await CreateTokenAsync(user),
                RefreshToken = await GenerateAndSaveRefreshTokenAsync(user)
            };
        }
    }
}
