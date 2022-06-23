using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    class WorkItemTypeConfiguration : IEntityTypeConfiguration<WorkItemType>
    {
        public void Configure(EntityTypeBuilder<WorkItemType> builder)
        {
            builder.ToTable("WorkItemTypes", "Lookups");

            builder.Property(m => m.Id)
                .ValueGeneratedNever();

            builder.Property(m => m.Name)
                .HasComment("The name of the WorkItemType")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(m => m.RolesIds)
                .HasComment("The roles IDs that will be able to use this work item type.")
                .HasMaxLength(1000)
                .IsRequired();

            builder.HasOne(m => m.Department)
               .WithMany(d => d.WorkItemTypes)
               .HasForeignKey(m => m.DepartmentId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
