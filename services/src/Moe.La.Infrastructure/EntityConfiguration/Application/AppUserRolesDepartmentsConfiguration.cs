using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    public class AppUserRolesDepartmentsConfiguration : IEntityTypeConfiguration<UserRoleDepartment>
    {
        public void Configure(EntityTypeBuilder<UserRoleDepartment> builder)
        {
            builder.ToTable("UserRolesDepartments", "Application");

            builder.HasOne(m => m.UserRole)
                .WithMany(d => d.UserRoleDepartmets)
                .HasForeignKey(m => new { m.UserId, m.RoleId })
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.Department)
                .WithMany()
                .HasForeignKey(m => m.DepartmentId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.CreatedByUser)
                .WithMany()
                .HasForeignKey(m => m.CreatedBy)
                .IsRequired();
        }
    }
}
