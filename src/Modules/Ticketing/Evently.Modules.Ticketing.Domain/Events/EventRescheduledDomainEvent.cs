using Evently.Common.Domain.Abstractions;

namespace Evently.Modules.Ticketing.Domain.Events;

public sealed class EventRescheduledDomainEvent(Guid eventId, DateTime startsAtUtc, DateTime? endsAtUtc)
    : DomainEvent
{
    public Guid EventId { get; init; } = eventId;
    public DateTime StartsAtUtc { get; init; } = startsAtUtc;
    public DateTime? EndsAtUtc { get; init; } = endsAtUtc;
}
