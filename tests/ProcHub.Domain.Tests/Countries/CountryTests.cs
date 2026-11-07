using ProcHub.Domain.Countries;
using ProcHub.Domain.Exceptions;
using ProcHub.Domain.Tests.Helpers;

namespace ProcHub.Domain.Tests.Countries;

public class CountryTests
{
    [Fact]
    public void Constructor_WithValidInput_InitializesCountry()
    {
        string testCode = "XX";
        string testName = "TEST";

        var country = new Country(
            testCode,
            testName);
        
        Assert.Equal(
            testCode,
            country.Code);
        
        Assert.Equal(
            testName,
            country.Name);
    }

    [Fact]
    public void Constructor_WhenWhiteSpace_TrimsCodeAndName()
    {
        string testCode = " X ";
        string trimmedCode = "X";
        string testName = "  TEST  ";
        string trimmedName = "TEST";

        var country = new Country(
            testCode,
            testName);
        
        Assert.Equal(
            trimmedCode,
            country.Code);
        
        Assert.Equal(
            trimmedName,
            country.Name);
    }

    [Fact]
    public void Constructor_WhenCodeLowerCase_ConvertsToUpperCase()
    {
        string testCode = "xx";
        string upperCaseCode = "XX";

        var country = new Country(
            testCode,
            "TEST");
        
        Assert.Equal(
            upperCaseCode,
            country.Code);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void Constructor_WithInvalidCode_ThrowDomainException(
        string? code)
    {
        Action act = () => new Country(
            code!,
            "TEST");
        
        Assert.Throws<DomainException>(act);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("     ")]
    public void Constructor_WithInvalidName_ThrowDomainException(
        string? name)
    {
        Action act = () => new Country(
            "XX",
            name!);
        
        Assert.Throws<DomainException>(act);
    }

    [Fact]
    public void Constructor_WithCodeOverMaxLength_ThrowDomainException()
    {
        var testCode = new string(
            'X',
            CountryConstants.CodeMaxLength + 1);
        
        Action act = () => new Country(
            testCode,
            "TEST");
        
        Assert.Throws<DomainException>(act);
    }

    [Fact]
    public void Constructor_WithNameOverMaxLength_ThrowDomainException()
    {
        var testName = new string(
            'X',
            CountryConstants.NameMaxLength + 1);
        
        Action act = () => new Country(
            "X",
            testName);
        
        Assert.Throws<DomainException>(act);
    }
}