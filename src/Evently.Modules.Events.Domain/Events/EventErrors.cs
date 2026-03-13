using Evently.Common.Domain.Abstractions;

namespace Evently.Modules.Events.Domain.Events;

public static class EventErrors
{
    public static readonly Error StartDateIntPast = Error.Problem(
        "Events.StartDateInPast",
        "The event cannot be scheduled in the past.");

    public static readonly Error EndDatePrecedesStartDate = Error.Problem(
        "Events.EndDatePrecedesStartDate",
        "The event end date precedes start date.");

    public static readonly Error NoTicketsFound = Error.Problem(
        "Event.NoTicketsFound",
        "The event does not have any ticket types defined.");

    public static readonly Error NotDraft = Error.Problem(
        "Events.NoDraft",
        "The event is not in draft status.");

    public static readonly Error AlreadyCanceled = Error.Problem(
        "Events.AlreadyCanceled",
        "The event was already canceled.");

    public static readonly Error AlreadyStarted = Error.Problem(
        "Events.AlreadyStarted",
        "The event has already started.");

    public static Error NotFound(Guid eventId)
    {
        return Error.NotFound("Events.NotFound", $"The event with the identifiers {eventId} was not found.");
    }
}
