using System.Security.Permissions;
using Microsoft.Extensions.DependencyInjection;

namespace ProcHub.Application.Features.Articles.ArticleGroups.Update;

public sealed record UpdateArticleGroupCommand
{
    public required string Name { get; init; }
    public string? Description { get; init; }
}