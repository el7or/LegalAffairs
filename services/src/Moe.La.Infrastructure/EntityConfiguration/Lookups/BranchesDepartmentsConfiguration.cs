using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    public class BranchesDepartmentsConfiguration : IEntityTypeConfiguration<BranchesDepartments>
    {
        public void Configure(EntityTypeBuilder<BranchesDepartments> builder)
        {
            builder.ToTable("BranchesDepartments", "Lookups");

            builder.HasKey(m => new { m.BranchId, m.DepartmentId });

            builder.HasOne(m => m.Branch)
                .WithMany(m => m.BranchDepartments)
                .HasForeignKey(m => m.BranchId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.Department)
                .WithMany()
                .HasForeignKey(m => m.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
