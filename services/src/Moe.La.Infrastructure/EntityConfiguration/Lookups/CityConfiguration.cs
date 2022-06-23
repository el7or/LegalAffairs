using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    class CityConfiguration : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            builder.ToTable("Cities", "Lookups");

            builder.Property(m => m.Id)
                .ValueGeneratedNever();

            builder.Property(m => m.Name)
                .HasComment("The name of the city")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(m => m.ProvinceId)
                .HasComment("The province that the city belongs to")
                .IsRequired();

            builder.HasOne(m => m.Province)
                .WithMany(m => m.Cities)
                .HasForeignKey(m => m.ProvinceId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
