using FluentValidation;

namespace Application.Users.Commands.Register
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Name)
                .NotEmpty()
                .Length(2, 100);

            RuleFor(x => x.Age)
                .GreaterThan(0)
                .LessThan(130);

            RuleFor(x => x.Password)
               .NotEmpty()
               .MinimumLength(6)
               .Matches("[0-9]");
        }
    }
}
