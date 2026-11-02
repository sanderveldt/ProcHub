using ProcHub.Domain.Exceptions;

namespace ProcHub.Domain.Articles.ArticleGroups;

public class ArticleGroup
{
    public int Id { get; private set; }
    public string Code { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }

    public ICollection<Article> Articles { get; private set; } 
        = new List<Article>();
    
    private ArticleGroup()
    {
    }

    public ArticleGroup(
        string code,
        string name)
    {
        SetCode(code);
        SetName(name);
    }

    private void SetCode(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            throw new DomainException("Article group code is required.");
        }

        if (code.Length > ArticleGroupConstants.CodeMaxLength)
        {
            throw new DomainException(
                "Article group code cannot exceed " + 
                $"{ArticleGroupConstants.CodeMaxLength} characters.");
        }

        Code = code.Trim();
    }

    private void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("Article group name is required.");
        }

        if (name.Length > ArticleGroupConstants.NameMaxLength)
        {
            throw new DomainException(
                "Article group name cannot exceed " + 
                $"{ArticleGroupConstants.NameMaxLength} characters.");
        }

        Name = name.Trim();
    }

    public void SetDescription(string? description)
    {
        if (description?.Length > ArticleGroupConstants.DescriptionMaxLength)
        {
            throw new DomainException(
                "Article group description cannot exceed " + 
                $"{ArticleGroupConstants.DescriptionMaxLength} characters.");
        }

        Description = description?.Trim();
    }
}
