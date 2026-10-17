using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace ProcHub.Infrastructure.Persistence.Configurations;

public sealed class UserRoleConfiguration
    : IEntityTypeConfiguration<IdentityUserRole<int>>
{
    public void Configure(
        EntityTypeBuilder<IdentityUserRole<int>> builder)
    {
        builder.HasIndex(userRole => userRole.UserId)
            .IsUnique();
    }
}