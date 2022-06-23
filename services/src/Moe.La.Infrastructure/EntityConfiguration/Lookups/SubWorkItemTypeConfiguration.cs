using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    class SubWorkItemTypeConfiguration : IEntityTypeConfiguration<SubWorkItemType>
    {
        public void Configure(EntityTypeBuilder<SubWorkItemType> builder)
        {
            builder.ToTable("SubWorkItemTypes", "Lookups");

            builder.Property(m => m.Id)
                .ValueGeneratedNever();

            builder.Property(m => m.Name)
                .HasComment("The name of the SubWorkItemType")
                .HasMaxLength(50)
                .IsRequired();

            builder.HasOne(m => m.WorkItemType)
               .WithMany(d => d.SubWorkItemTypes)
               .HasForeignKey(m => m.WorkItemTypeId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);
        }
    }
}