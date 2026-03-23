using FluentValidation;

namespace Evently.Modules.Events.Application.Events.PublishEvent;

internal sealed class PublishEventCommandValidator : AbstractValidator<PublishEventCommand>
{
    public PublishEventCommandValidator()
    {
        RuleFor(p => p.EventId)
            .NotEmpty();
    }
}
