using FluentValidation;

namespace Evently.Modules.Users.Application.Users.UpdateUser;

internal sealed class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(u => u.UserId)
            .NotEmpty();

        RuleFor(u => u.FirstName)
            .NotEmpty();

        RuleFor(u => u.LastName)
            .NotEmpty();
    }
}
