using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProcHub.Domain.Articles;
using ProcHub.Domain.Articles.ArticleGroups;

namespace ProcHub.Infrastructure.Persistence.Configurations;

public sealed class ArticleConfiguration
    : IEntityTypeConfiguration<Article>
{
    public void Configure(EntityTypeBuilder<Article> builder)
    {
        builder.ToTable("Articles");

        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id)
            .ValueGeneratedOnAdd();

        builder.HasIndex(a => a.Code)
            .IsUnique();
        builder.Property(a => a.Code)
            .IsRequired()
            .HasMaxLength(ArticleConstants.CodeMaxLength);

        builder.Property(a => a.Description)
            .HasMaxLength(ArticleConstants.DescriptionMaxLength);

        builder.Property(a => a.Status)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(a => a.PurchasePrice)
            .HasPrecision(18, 2)
            .IsRequired();
        
        builder.HasOne(a => a.ArticleGroup)
            .WithMany(ag => ag.Articles)
            .HasForeignKey(a => a.ArticleGroupId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.Supplier)
            .WithMany()
            .HasForeignKey(a => a.SupplierId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.Property(po => po.Version)
            .IsRowVersion();
    }
}