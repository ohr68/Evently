using Evently.Common.Domain.Abstractions;

namespace Evently.Modules.Ticketing.Domain.Tickets;

public static class TicketErrors
{
    public static Error NotFound(Guid ticketId)
    {
        return Error.NotFound("Tickets.NotFound", $"The ticket with the identifier {ticketId} was not found.");
    }

    public static Error NotFound(string code)
    {
        return Error.NotFound("Tickets.NotFound", $"The ticket with the code {code} was not found.");
    }
}
