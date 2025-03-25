using Application.Users.Commands.Common;
using Application.Users.Commands.RefreshTokens;
using Domain.Entities;

namespace Application.Services.TokenService
{
    public interface ITokenService
    {
        Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokensCommand request);

        Task<TokenResponseDto> CreateTokenResponse(User user);
    }
}
