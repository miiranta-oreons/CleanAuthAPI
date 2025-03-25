using Application.Common;
using Domain.Constants;
using Domain.Entities;
using FluentValidation;
using Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Commands.Register
{
    class RegisterCommandHandler(AppDbContext context, IValidator<RegisterCommand> validator) : IRequestHandler<RegisterCommand, ControllerResult<User>>
    {
        public async Task<ControllerResult<User>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            if (validator is null)
            {
                throw new ArgumentNullException(nameof(validator));
            }

            var validationResult = validator.Validate(request);
            if (!validationResult.IsValid)
            {
                return ControllerResultBuilder.Reject<User>(string.Join(",\n", validationResult.Errors.Select(x => x.ErrorMessage)) + ".");
            }

            if (await context.Users.AnyAsync(x => x.Email == request.Email))
            {
                return ControllerResultBuilder.Reject<User>(ErrorMessages.UserAlreadyExists);
            }

            var user = new User()
            {
                Email = string.Empty,
                PasswordHash = string.Empty
            };

            var hashedPassword = new PasswordHasher<User>()
           .HashPassword(user, request.Password);

            user.Name = request.Name;
            user.Age = request.Age;
            user.Email = request.Email;
            user.PasswordHash = hashedPassword;

            context.Users.Add(user);
            await context.SaveChangesAsync();

            return ControllerResultBuilder.Resolve(user);
        }

    }
}
