using Application.Common;
using Application.Users.Commands.Common;
using MediatR;

namespace Application.Users.Commands.RefreshTokens
{
    public class RefreshTokensCommand : IRequest<ControllerResult<TokenResponseDto>>
    {
        public Guid UserId { get; set; }

        public required string RefreshToken { get; set; }
    }
}
