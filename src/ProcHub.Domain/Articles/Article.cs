using ProcHub.Domain.Exceptions;
using ProcHub.Domain.Suppliers;
using ProcHub.Domain.Articles.ArticleGroups;

namespace ProcHub.Domain.Articles;

public class Article
{
    public int Id { get; private set; }
    public string Code { get; private set; } = null!;
    public DateOnly CreationDate { get; private set; } 
        = DateOnly.FromDateTime(DateTime.Today);
    public ArticleStatus Status { get; private set; } 
        = ArticleStatus.Active;

    public string Description { get; private set; } = null!;
    public decimal PurchasePrice { get; private set; }

    public int ArticleGroupId { get; private set; }
    public ArticleGroup ArticleGroup { get; private set; } = null!;

    public int SupplierId { get; private set; }
    public Supplier Supplier { get; private set; } = null!;

    private Article()
    {
    }

    public Article(
        string code,
        string description,
        decimal purchasePrice,
        ArticleGroup articleGroup,
        Supplier supplier)
    {
        SetCode(code);
        SetDescription(description);
        SetPurchasePrice(purchasePrice);
        SetArticleGroup(articleGroup);
        SetSupplier(supplier);
    }

    private void SetCode(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            throw new DomainException("Article code is required.");
        }

        if (code.Length > ArticleConstants.CodeMaxLength)
        {
            throw new DomainException(
                "Article code cannot exceed " + 
                $"{ArticleConstants.CodeMaxLength} characters.");
        }

        Code = code.Trim();
    }

    public void SetDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            throw new DomainException("Article description is required.");
        }

        if (description.Length > ArticleConstants.DescriptionMaxLength)
        {
            throw new DomainException(
                "Article description cannot exceed " + 
                $"{ArticleConstants.DescriptionMaxLength} characters.");
        }

        Description = description.Trim();
    }

    public void SetPurchasePrice(decimal purchasePrice)
    {
        if (purchasePrice < 0)
        {
            throw new DomainException(
                "Article purchase price cannot be negative.");
        }

        if (purchasePrice != decimal.Round(purchasePrice, 2))
        {
            throw new DomainException(
                "Article purchase price cannot have more than two decimal places.");
        }

        PurchasePrice = purchasePrice;
    }

    public void SetArticleGroup(ArticleGroup articleGroup)
    {
        if (articleGroup == null)
        {
            throw new DomainException(
                "Article group is required.");
        }

        ArticleGroup = articleGroup;
    }

    public void SetSupplier(Supplier supplier)
    {
        if (supplier == null)
        {
            throw new DomainException(
                "Supplier is required.");
        }

        Supplier = supplier;
    }




    
}