using ProcHub.Domain.Articles.ArticleGroups;
using ProcHub.Domain.Exceptions;
using ProcHub.Domain.Tests.Helpers;

namespace ProcHub.Domain.Tests.Articles.ArticleGroups;

public class ArticleGroupTests
{
    [Fact]
    public void Constructor_WithValidInput_InitializesArticleGroup()
    {
        string testCode = "32";
        string testName = "TEST";

        var articleGroup = new ArticleGroup(
            testCode,
            testName);
        
        Assert.Equal(
            testCode,
            articleGroup.Code);
        
        Assert.Equal(
            testName,
            articleGroup.Name);
    }

    [Fact]
    public void Constructor_WithOutsideWhiteSpace_TrimsCodeAndName()
    {
        string testCode = " 32 ";
        string trimmedCode = "32";
        string testName = " TEST ";
        string trimmedName = "TEST";
        
        var articleGroup = new ArticleGroup(
            testCode,
            testName);
        
        Assert.Equal(
            trimmedCode,
            articleGroup.Code);
        
        Assert.Equal(
            trimmedName,
            articleGroup.Name);
    }

    [Theory]
    [InlineData(" ")]
    [InlineData("  ")]
    [InlineData(null)]
    public void Constructor_WithCodeNullOrWhiteSpace_ThrowDomainException(
        string? code)
    {
        Action act = () => new ArticleGroup(
            code!,
            "TEST");

        Assert.Throws<DomainException>(act);
    }

    [Theory]
    [InlineData("  ")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Constructor_WithNameNullOrWhiteSpace_ThrowDomainException(
        string? name)
    {
        Action act = () => new ArticleGroup(
            "32",
            name!);

        Assert.Throws<DomainException>(act);
    }

    [Fact]
    public void Constructor_WithCodeOverMaxLength_ThrowDomainException()
    {
        var testCode = new string(
            'X',
            ArticleGroupConstants.CodeMaxLength + 1);
        
        Action act = () => new ArticleGroup(
            testCode,
            "TEST");
        
        Assert.Throws<DomainException>(act);
    }

    [Fact]
    public void Constructor_WithNameOverMaxLength_ThrowDomainException()
    {
        var testName = new string(
            'X',
            ArticleGroupConstants.NameMaxLength + 1);
        
        Action act = () => new ArticleGroup(
            testName,
            "TEST");
        
        Assert.Throws<DomainException>(act);
    }

    [Fact]
    public void SetDescription_WithValidInput_SetDescription()
    {
        string testDescription = "Test Description";
        var articleGroup = DomainBuilder
            .CreateArticleGroup();

        articleGroup.SetDescription(testDescription);

        Assert.Equal(
            testDescription,
            articleGroup.Description);
    }

    [Fact]
    public void SetDescription_WithOutsideWhiteSpace_TrimsDescription()
    {
        string testDescription = "  TEST  ";
        string trimmedDescription = "TEST";
        var articleGroup = DomainBuilder
            .CreateArticleGroup();
        
        articleGroup.SetDescription(testDescription);

        Assert.Equal(
            trimmedDescription,
            articleGroup.Description);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("  ")]
    [InlineData("    ")]
    public void SetDescription_WhenNullOrWhiteSpace_SetToNull(
        string? description)
    {
        var articleGroup = DomainBuilder
            .CreateArticleGroup();

        articleGroup.SetDescription(description);

        Assert.Null(articleGroup.Description);
    }

    [Fact]
    public void SetDescription_WhenOverMaxLength_ThrowDomainException()
    {
        var articleGroup = DomainBuilder
            .CreateArticleGroup();
        var testDescription = new string(
            'X',
            ArticleGroupConstants.DescriptionMaxLength + 1);
        
        Action act = () => articleGroup
            .SetDescription(testDescription);
        
        Assert.Throws<DomainException>(act);
    }
}