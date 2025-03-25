using Application.Common;
using Application.Services.TokenService;
using Application.Users.Commands.Common;
using Domain.Constants;
using MediatR;

namespace Application.Users.Commands.RefreshTokens
{
    class RefreshTokensCommandHandler(ITokenService tokenService) : IRequestHandler<RefreshTokensCommand, ControllerResult<TokenResponseDto>>
    {
        public async Task<ControllerResult<TokenResponseDto>> Handle(RefreshTokensCommand request, CancellationToken cancellationToken)
        {
            var tokens = await tokenService.RefreshTokensAsync(request);

            if(tokens is null)
            {
                return ControllerResultBuilder.Reject<TokenResponseDto>(ErrorMessages.InvalidTokenOrUserId);
            }

            return ControllerResultBuilder.Resolve(tokens);
        }
    }
}
