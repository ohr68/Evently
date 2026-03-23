namespace Evently.Common.Domain.Abstractions;

public interface IDomainEvent
{
    Guid Id { get; }

    DateTime OccuredOnUtc { get; }
}
