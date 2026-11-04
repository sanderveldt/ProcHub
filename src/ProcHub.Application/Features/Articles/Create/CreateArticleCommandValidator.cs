using FluentValidation;
using ProcHub.Domain.Articles;

namespace ProcHub.Application.Features.Articles.Create;

public sealed class CreateArticleCommandValidator 
    : AbstractValidator<CreateArticleCommand>
{
    public CreateArticleCommandValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage("Article code is required.")
            .MaximumLength(ArticleConstants.CodeMaxLength)
            .WithMessage("Article code cannot exceed " +
                        $"{ArticleConstants.CodeMaxLength} characters.");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Article description is required.")
            .MaximumLength(ArticleConstants.DescriptionMaxLength)
            .WithMessage("Article description cannot exceed " +
                        $"{ArticleConstants.DescriptionMaxLength} characters.");

        RuleFor(x => x.PurchasePrice)
            .GreaterThan(0)
            .WithMessage("Purchase price must be a positive value.")
            .PrecisionScale(18, 2, true)
            .WithMessage("Article purchase price cannot have more than two decimal places.");

        RuleFor(x => x.OrderCode)
            .MaximumLength(ArticleConstants.OrderCodeMaxLength)
            .WithMessage("Order code cannot exceed " +
                        $"{ArticleConstants.OrderCodeMaxLength} characters.");

        RuleFor(x => x.EanCode)
            .MaximumLength(ArticleConstants.EanCodeMaxLength)
            .WithMessage("EAN code cannot exceed " +
                        $"{ArticleConstants.EanCodeMaxLength} characters.");
        
        RuleFor(x => x.ArticleGroupId)
            .NotEmpty()
            .WithMessage("Article group is required.")
            .GreaterThan(0);
        
        RuleFor(x => x.SupplierId)
            .NotEmpty()
            .WithMessage("Supplier is required.")
            .GreaterThan(0);     
    }
}
