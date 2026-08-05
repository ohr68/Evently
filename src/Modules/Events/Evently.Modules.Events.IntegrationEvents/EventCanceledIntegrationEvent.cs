using Evently.Common.Application.EventBus;

namespace Evently.Modules.Events.IntegrationEvents;

public sealed class EventCanceledIntegrationEvent : IntegrationEvent
{
    public EventCanceledIntegrationEvent(Guid id, DateTime occuredOnUtc, Guid eventId)
        : base(id, occuredOnUtc)
    {
        EventId = eventId;
    }

    public Guid EventId { get; init; }
}
