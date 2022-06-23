using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    class DistrictConfiguration : IEntityTypeConfiguration<District>
    {
        public void Configure(EntityTypeBuilder<District> builder)
        {
            builder.ToTable("Districts", "Lookups");

            builder.Property(m => m.Id)
                .ValueGeneratedNever();

            builder.Property(m => m.Name)
                .HasComment("The name of the District")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(m => m.CityId)
                .HasComment("The city that the District belongs to")
                .IsRequired();

            builder.HasOne(m => m.City)
                .WithMany(m => m.Districts)
                .HasForeignKey(m => m.CityId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
