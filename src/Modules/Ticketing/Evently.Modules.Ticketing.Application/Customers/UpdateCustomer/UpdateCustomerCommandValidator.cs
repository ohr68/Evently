using FluentValidation;

namespace Evently.Modules.Ticketing.Application.Customers.UpdateCustomer;

internal sealed class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
{
    public UpdateCustomerCommandValidator()
    {
        RuleFor(c => c.CustomerId).NotEmpty();
        RuleFor(command => command.FirstName).NotEmpty();
        RuleFor(command => command.LastName).NotEmpty();
    }
}
