using FluentValidation;
using ProcHub.Domain.Articles.ArticleGroups;

namespace ProcHub.Application.Features.Articles.ArticleGroups.Update;

public sealed class UpdateArticleGroupValidator
    : AbstractValidator<UpdateArticleGroupCommand>
{
    public UpdateArticleGroupValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("ArticleGroup name is required.")
            .MaximumLength(ArticleGroupConstants.NameMaxLength)
            .WithMessage("ArticleGroup name cannot exceed " +
                        $"{ArticleGroupConstants.NameMaxLength} characters.");
        
        RuleFor(x => x.Description)
            .MaximumLength(ArticleGroupConstants.DescriptionMaxLength)
            .WithMessage("ArticleGroup description cannot exceed " +
                        $"{ArticleGroupConstants.DescriptionMaxLength} characters.");
    }
}