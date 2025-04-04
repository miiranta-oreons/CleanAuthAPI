﻿using Application.Common;
using Application.Services.TokenService;
using Application.Users.Commands.Common;
using Domain.Constants;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Commands.Login
{
    class LoginCommandHandler(ITokenService tokenService, IValidator<LoginCommand> validator, UserManager<User> userManager) : IRequestHandler<LoginCommand, ControllerResult<TokenResponseDto>>
    {
        public async Task<ControllerResult<TokenResponseDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            if (validator is null)
            {
                throw new ArgumentNullException(nameof(validator));
            }

            var validationResult = validator.Validate(request);
            if (!validationResult.IsValid)
            {
                return ControllerResultBuilder.Reject<TokenResponseDto>(string.Join("\n", validationResult.Errors.Select(x => x.ErrorMessage)));
            }

            var user = await userManager.Users.FirstOrDefaultAsync(x => x.Email == request.Email);

            if (user is null)
            {
                return ControllerResultBuilder.Reject<TokenResponseDto>(ErrorMessages.UserNotFound);
            }

            if (new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, request.Password) == PasswordVerificationResult.Failed)
            {
                return ControllerResultBuilder.Reject<TokenResponseDto>(ErrorMessages.InvalidCredentials);
            }

            var tokens = await tokenService.CreateTokenResponse(user);

            return ControllerResultBuilder.Resolve(tokens);
        }

    }
}
