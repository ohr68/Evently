using Evently.Common.Domain.Abstractions;

namespace Evently.Modules.Ticketing.Domain.Orders;

public static class OrderErrors
{
    public static readonly Error TicketsAlreadyIssued = Error.Problem(
        "Order.TicketsAlreadyIssued",
        "The tickets for this order were already issued.");

    public static Error NotFound(Guid orderId)
    {
        return Error.NotFound("Orders.NotFound", $"The order with the identifier {orderId} was not found.");
    }
}
