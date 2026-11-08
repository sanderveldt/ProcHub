using System.Security.Permissions;
using ProcHub.Domain.Articles;
using ProcHub.Domain.Articles.ArticleGroups;
using ProcHub.Domain.Exceptions;
using ProcHub.Domain.Suppliers;
using ProcHub.Domain.Tests.Helpers;

namespace ProcHub.Domain.Tests.Articles;

public class ArticleTests
{
    public static ArticleGroup ArticleGroup { get; }
        = DomainBuilder.CreateArticleGroup();

    public static Supplier Supplier { get; }
        = DomainBuilder.CreateSupplier();

    [Fact]
    public void Constructor_WithValidInput_InitializeArticle()
    {
        string testCode = "32";
        string testDescription = "TestDescription";
        decimal testPrice = 32.32m;
        var articleGroup = ArticleGroup;
        var supplier = Supplier;
        
        var article = new Article(
            testCode,
            testDescription,
            testPrice,
            articleGroup,
            supplier);
        
        Assert.Equal(
            testCode,
            article.Code); 
        Assert.Equal(
            testDescription,
            article.Description);
        Assert.Equal(
            testPrice,
            article.PurchasePrice);
        
        Assert.Same(
            articleGroup,
            article.ArticleGroup);
        Assert.Same(
            supplier,
            article.Supplier);
    }

    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData(null)]
    public void Constructor_WhenCodeIsNullOrWhiteSpace_ThrowDomainException(
        string? testCode)
    {
 
        Action act = () => new Article(
            testCode!,
            "TestDescription",
            32.32m,
            ArticleGroup,
            Supplier);
        
        Assert.Throws<DomainException>(act);
    }

    [Fact]
    public void Constructor_WhenCodeOverMaxLength_ThrowDomainException()
    {
        var testCode = new string(
            'X',
            ArticleConstants.CodeMaxLength + 1);
        
        Action act = () => new Article(
            testCode,
            "TestDescription",
            32.32m,
            ArticleGroup,
            Supplier);
        
        Assert.Throws<DomainException>(act);
    }





}