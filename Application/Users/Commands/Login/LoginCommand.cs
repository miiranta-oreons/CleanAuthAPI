using Application.Common;
using Application.Users.Commands.Common;
using MediatR;

namespace Application.Users.Commands.Login
{
    public class LoginCommand : IRequest<ControllerResult<TokenResponseDto>>
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
