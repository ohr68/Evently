using Evently.Common.Domain.Abstractions;
using Evently.Modules.Events.Domain.Categories;
using Evently.Modules.Events.Domain.Events;
using Evently.Modules.Events.UnitTests.Abstractions;
using FluentAssertions;

namespace Evently.Modules.Events.UnitTests.Events;

public class EventTests : BaseTest
{
    [Fact]
    public void Create_ShouldReturnFailure_WhenEndDatePrecedesStartDate()
    {
        // Arrange
        var category = Category.Create(Faker.Music.Genre());
        DateTime startsAtUtc = DateTime.UtcNow;
        DateTime endsAtUtc = startsAtUtc.AddMinutes(-1);
        
        // Act
        Result<Event> result = Event.Create(
            category,
            Faker.Music.Genre(),
            Faker.Music.Genre(),
            Faker.Address.StreetAddress(),
            startsAtUtc,
            endsAtUtc);

        // Assert
        result.Error.Should().Be(EventErrors.EndDatePrecedesStartDate);
    }
    
    [Fact]
    public void Create_ShouldRaiseDomainEvent_WhenEventCreated()
    {
        // Arrange
        var category = Category.Create(Faker.Music.Genre());
        DateTime startsAtUtc = DateTime.UtcNow;
        
        Result<Event> result = Event.Create(
            category,
            Faker.Music.Genre(),
            Faker.Music.Genre(),
            Faker.Address.StreetAddress(),
            startsAtUtc,
            null);
        
        Event @event = result.Value;

        // Act
        EventCreatedDomainEvent eventCreatedDomainEvent = AssertDomainEventWasPublished<EventCreatedDomainEvent>(@event);
        
        // Assert
        eventCreatedDomainEvent.EventId.Should().Be(@event.Id);
    }
    
    [Fact]
    public void Publish_ShouldReturnFailure_WhenEventNoDraft()
    {
        // Arrange
        var category = Category.Create(Faker.Music.Genre());
        DateTime startsAtUtc = DateTime.UtcNow;
        
        Result<Event> result = Event.Create(
            category,
            Faker.Music.Genre(),
            Faker.Music.Genre(),
            Faker.Address.StreetAddress(),
            startsAtUtc,
            null);
        
        Event @event = result.Value;

        @event.Publish();

        // Act
        Result publishResult = @event.Publish();
        
        // Assert
        publishResult.Error.Should().Be(EventErrors.NotDraft);
    }
    
    [Fact]
    public void Publish_ShouldRaiseDomainEvent_WhenEventPublished()
    {
        // Arrange
        var category = Category.Create(Faker.Music.Genre());
        DateTime startsAtUtc = DateTime.UtcNow;
        
        Result<Event> result = Event.Create(
            category,
            Faker.Music.Genre(),
            Faker.Music.Genre(),
            Faker.Address.StreetAddress(),
            startsAtUtc,
            null);
        
        Event @event = result.Value;

        // Act
        @event.Publish();

        EventPublishedDomainEvent eventPublishedDomainEvent = AssertDomainEventWasPublished<EventPublishedDomainEvent>(@event);
        
        // Assert
        eventPublishedDomainEvent.EventId.Should().Be(@event.Id);
    }
    
    [Fact]
    public void Reschedule_ShouldRaiseDomainEvent_WhenEventRescheduled()
    {
        // Arrange
        var category = Category.Create(Faker.Music.Genre());
        DateTime startsAtUtc = DateTime.UtcNow;
        
        Result<Event> result = Event.Create(
            category,
            Faker.Music.Genre(),
            Faker.Music.Genre(),
            Faker.Address.StreetAddress(),
            startsAtUtc,
            null);
        
        Event @event = result.Value;

        // Act
        @event.Reschedule(startsAtUtc.AddDays(1), startsAtUtc.AddDays(2));

        EventRescheduledDomainEvent eventRescheduledDomainEvent = AssertDomainEventWasPublished<EventRescheduledDomainEvent>
            (@event);
        
        // Assert
        eventRescheduledDomainEvent.EventId.Should().Be(@event.Id);
    }
    
    [Fact]
    public void Cancel_ShouldRaiseDomainEvent_WhenEventCanceled()
    {
        // Arrange
        var category = Category.Create(Faker.Music.Genre());
        DateTime startsAtUtc = DateTime.UtcNow;
        
        Result<Event> result = Event.Create(
            category,
            Faker.Music.Genre(),
            Faker.Music.Genre(),
            Faker.Address.StreetAddress(),
            startsAtUtc,
            null);
        
        Event @event = result.Value;

        
        // Act
        @event.Cancel(startsAtUtc.AddMinutes(-1));

        EventCanceledDomainEvent eventCanceledDomainEvent = AssertDomainEventWasPublished<EventCanceledDomainEvent>
            (@event);
        
        
        // Assert
        eventCanceledDomainEvent.EventId.Should().Be(@event.Id);
    }
    
    [Fact]
    public void Cancel_ShouldReturnFailure_WhenEventAlreadyCanceled()
    {
        // Arrange
        var category = Category.Create(Faker.Music.Genre());
        DateTime startsAtUtc = DateTime.UtcNow;
        
        Result<Event> result = Event.Create(
            category,
            Faker.Music.Genre(),
            Faker.Music.Genre(),
            Faker.Address.StreetAddress(),
            startsAtUtc,
            null);
        
        Event @event = result.Value;

        @event.Cancel(startsAtUtc.AddMinutes(-1));
        
        // Act
        Result cancelResult = @event.Cancel(startsAtUtc.AddMinutes(-1));
        
        // Assert
        cancelResult.Error.Should().Be(EventErrors.AlreadyCanceled);
        
    }

    [Fact]
    public void Cancel_ShouldReturnFailure_WhenEventAlreadyStarted()
    {
        // Arrange
        var category = Category.Create(Faker.Music.Genre());
        DateTime startsAtUtc = DateTime.UtcNow;
        
        Result<Event> result = Event.Create(
            category,
            Faker.Music.Genre(),
            Faker.Music.Genre(),
            Faker.Address.StreetAddress(),
            startsAtUtc,
            null);
        
        Event @event = result.Value;
        
        // Act
        Result cancelResult = @event.Cancel(startsAtUtc.AddMinutes(1));
        
        // Assert
        cancelResult.Error.Should().Be(EventErrors.AlreadyStarted);
    }
}
