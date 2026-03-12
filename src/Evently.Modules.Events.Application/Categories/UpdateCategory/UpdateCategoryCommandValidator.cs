using FluentValidation;

namespace Evently.Modules.Events.Application.Categories.UpdateCategory;

internal sealed class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator()
    {
        RuleFor(u => u.CategoryId)
            .NotEmpty();

        RuleFor(u => u.Name)
            .NotEmpty();
    }
}
