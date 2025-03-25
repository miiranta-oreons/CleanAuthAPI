using Application.Common;
using Domain.Entities;
using MediatR;

namespace Application.Users.Commands.Register
{
    public class RegisterCommand : IRequest<ControllerResult<User>>
    {

        // Not "useful" stuff
        public string? Name { get; set; }

        public int? Age { get; set; }


        // "Useful" stuff
        public required string Email { get; set; }

        public required string Password { get; set; } //plain, not hash

    }
}

