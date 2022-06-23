using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.ToTable("Countries", "Lookups");

            builder.Property(m => m.Id)
                .ValueGeneratedNever();

            builder.Property(m => m.NameAr)
                .HasComment("Country Arabic name")
                .HasMaxLength(60)
                .IsRequired();

            builder.Property(m => m.NameEn)
                .HasComment("Country English name")
                .HasMaxLength(60)
                .IsRequired();

            builder.Property(m => m.NationalityAr)
                .HasComment("Nationality Arabic name")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(m => m.NationalityEn)
                .HasComment("Nationality English name")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(m => m.ISO31661CodeAlph3)
                .HasComment("Country ISO 31661 Code Alph3")
                .HasMaxLength(3)
                .IsRequired();

        }
    }
}
