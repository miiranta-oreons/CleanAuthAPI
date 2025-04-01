using Application.Common;
using Domain.Constants;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Users.Commands.Register
{
    class RegisterCommandHandler(UserManager<User> userManager, IValidator<RegisterCommand> validator) : IRequestHandler<RegisterCommand, ControllerResult<User>>
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
                return ControllerResultBuilder.Reject<User>(string.Join("\n", validationResult.Errors.Select(x => x.ErrorMessage)));
            }

            if (await userManager.FindByEmailAsync(request.Email) is not null)
            {
                return ControllerResultBuilder.Reject<User>(ErrorMessages.UserAlreadyExists);
            }

            var user = new User()
            {
                Email = request.Email,
                UserName = request.Email,
                PasswordHash = string.Empty,
                Name = request.Name,
                Age = request.Age
            };

            var hashedPassword = new PasswordHasher<User>()
                .HashPassword(user, request.Password);
            user.PasswordHash = hashedPassword;

            var userCreated = await userManager.CreateAsync(user);
            if (!userCreated.Succeeded)
            {
                return ControllerResultBuilder.Reject<User>(ErrorMessages.InternalServerError);
            }

            var defaultRoleAdded = await userManager.AddToRoleAsync(user: user, role: RoleTypes.Default);
            if (!defaultRoleAdded.Succeeded)
            {
                return ControllerResultBuilder.Reject<User>(ErrorMessages.InternalServerError);
            }

            // Testing admin role
            //var adminRoleAdded = await userManager.AddToRoleAsync(user: user, role: RoleTypes.Admin);
            //if (!adminRoleAdded.Succeeded)
            //{
            //    return ControllerResultBuilder.Reject<User>(ErrorMessages.InternalServerError);
            //}

            return ControllerResultBuilder.Resolve(user);
        }

    }
}
