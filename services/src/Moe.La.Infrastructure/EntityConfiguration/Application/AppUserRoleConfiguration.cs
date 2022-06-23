using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    public class AppUserRoleConfiguration : IEntityTypeConfiguration<AppUserRole>
    {
        public void Configure(EntityTypeBuilder<AppUserRole> builder)
        {
            builder.ToTable("UserRoles", "Application");

            builder.HasKey(m => new { m.UserId, m.RoleId });

            builder.HasOne(m => m.User)
                .WithMany(m => m.UserRoles)
                .HasForeignKey(m => m.UserId)
                .IsRequired();

            builder.HasOne(m => m.Role)
                .WithMany(m => m.UserRoles)
                .HasForeignKey(m => m.RoleId)
                .IsRequired();
        }
    }
}
