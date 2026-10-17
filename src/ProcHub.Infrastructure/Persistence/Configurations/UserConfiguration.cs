using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.AspNetCore.Identity;
using ProcHub.Application.Common.Constants;
using Microsoft.EntityFrameworkCore;
using ProcHub.Infrastructure.Identity;

namespace ProcHub.Infrastructure.Persistence.Configurations;

public sealed class UserConfiguration
    : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(
        EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(u => u.UserName)
            .HasMaxLength(UserConstants.UserNameMaxLength);

        builder.Property(u => u.NormalizedUserName)
            .HasMaxLength(UserConstants.UserNameMaxLength);

        builder.Property(u => u.Email)
            .HasMaxLength(UserConstants.EmailMaxLength);

        builder.HasIndex(u => u.NormalizedEmail)
            .IsUnique();
        builder.Property(u => u.NormalizedEmail)
            .HasMaxLength(UserConstants.EmailMaxLength);

        builder.Property(user => user.DisplayName)
            .HasMaxLength(UserConstants.DisplayNameMaxLength);
    }
}