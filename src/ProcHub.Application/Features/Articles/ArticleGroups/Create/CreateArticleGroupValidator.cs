using FluentValidation;
using ProcHub.Domain.Articles.ArticleGroups;

namespace ProcHub.Application.Features.Articles.ArticleGroups.Create;

public sealed class CreateArticleGroupValidator
    : AbstractValidator<CreateArticleGroupCommand>
{
    public CreateArticleGroupValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage("ArticleGroup code is required.")
            .MaximumLength(ArticleGroupConstants.CodeMaxLength)
            .WithMessage("ArticleGroup code cannot exceed " +
                        $"{ArticleGroupConstants.CodeMaxLength} characters.");
        
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