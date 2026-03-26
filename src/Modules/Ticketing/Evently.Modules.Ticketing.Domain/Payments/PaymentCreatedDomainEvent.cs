using Evently.Common.Domain.Abstractions;

namespace Evently.Modules.Ticketing.Domain.Payments;

public sealed class PaymentCreatedDomainEvent(Guid paymentId) : DomainEvent
{
    public Guid PaymentId { get; private set; } = paymentId;
}
