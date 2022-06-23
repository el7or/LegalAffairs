using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    class SecondSubCategoriesConfiguration : IEntityTypeConfiguration<SecondSubCategory>
    {
        public void Configure(EntityTypeBuilder<SecondSubCategory> builder)
        {
            builder.ToTable("SecondSubCategories", "Lookups");

            builder.Property(m => m.Id)
                .ValueGeneratedNever();

            builder.Property(m => m.Name)
                .HasComment("The name of the second sub category")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(m => m.FirstSubCategoryId)
                .HasComment("The first sub category that the second sub category belongs to")
                .IsRequired();

            builder.HasOne(m => m.FirstSubCategory)
                .WithMany(m => m.SecondSubCategories)
                .HasForeignKey(m => m.FirstSubCategoryId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
