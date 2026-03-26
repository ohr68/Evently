using FluentValidation;

namespace Evently.Modules.Ticketing.Application.Carts.AddItemToCart;

internal sealed class AddItemToCartCommandValidator : AbstractValidator<AddItemToCartCommand>
{
    public AddItemToCartCommandValidator()
    {
        RuleFor(a => a.CustomerId).NotEmpty();
        RuleFor(a => a.TicketTypeId).NotEmpty();
        RuleFor(a => a.Quantity).NotEmpty();
    }
}
