using FluentValidation;

namespace Evently.Modules.Ticketing.Application.Events.RescheduleEvent;

internal sealed class RescheduleEventCommandValidator : AbstractValidator<RescheduleEventCommand>
{
    public RescheduleEventCommandValidator()
    {
        RuleFor(r => r.EventId).NotEmpty();
        RuleFor(r => r.StartsAtUtc).NotEmpty();
        RuleFor(r => r.EndsAtUtc)
            .Must((cmd, endsAt) => endsAt > cmd.StartsAtUtc)
            .When(r => r.EndsAtUtc.HasValue);
    }
}
