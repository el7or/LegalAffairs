using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    class FirstSubCategoryConfiguration : IEntityTypeConfiguration<FirstSubCategory>
    {
        public void Configure(EntityTypeBuilder<FirstSubCategory> builder)
        {
            builder.ToTable("FirstSubCategories", "Lookups");

            builder.Property(m => m.Id)
                .ValueGeneratedNever();

            builder.Property(m => m.Name)
                .HasComment("The name of the first sub category")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(m => m.MainCategoryId)
                .HasComment("The main category that the first sub category belongs to")
                .IsRequired();

            builder.HasOne(m => m.MainCategory)
                .WithMany(m => m.FirstSubCategories)
                .HasForeignKey(m => m.MainCategoryId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
