using FluentValidation;

namespace Evently.Modules.Events.Application.TicketTypes.UpdateTicketTypePrice;

internal sealed class UpdateTicketTypePriceCommandValidator : AbstractValidator<UpdateTicketTypePriceCommand>
{
    public UpdateTicketTypePriceCommandValidator()
    {
        RuleFor(u => u.TicketTypeId)
            .NotEmpty();

        RuleFor(u => u.Price)
            .GreaterThan(decimal.Zero);
    }
}
