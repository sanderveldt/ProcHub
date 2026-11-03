using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProcHub.Domain.Articles;
using ProcHub.Domain.Articles.ArticleGroups;

namespace ProcHub.Infrastructure.Persistence.Configurations;

public sealed class ArticleGroupConfiguration
    : IEntityTypeConfiguration<ArticleGroup>
{
    public void Configure(EntityTypeBuilder<ArticleGroup> builder)
    {
        builder.ToTable("ArticleGroups");

        builder.HasKey(ag => ag.Id);
        builder.Property(ag => ag.Id)
            .ValueGeneratedOnAdd();

        builder.HasIndex(ag => ag.Code)
            .IsUnique();
        builder.Property(ag => ag.Code)
            .IsRequired()
            .HasMaxLength(ArticleGroupConstants.CodeMaxLength);

        builder.Property(ag => ag.Name)
            .IsRequired()
            .HasMaxLength(ArticleGroupConstants.NameMaxLength);

        builder.Property(ag => ag.Description)
            .HasMaxLength(ArticleGroupConstants.DescriptionMaxLength);
    }
}