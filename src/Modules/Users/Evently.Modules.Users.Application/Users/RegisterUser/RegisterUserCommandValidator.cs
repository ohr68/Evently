using FluentValidation;

namespace Evently.Modules.Users.Application.Users.RegisterUser;

internal sealed class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(r => r.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(r => r.FirstName)
            .NotEmpty();

        RuleFor(r => r.LastName)
            .NotEmpty();
    }
}
