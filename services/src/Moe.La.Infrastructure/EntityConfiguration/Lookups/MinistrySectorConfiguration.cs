using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    public class MinistrySectorConfiguration : IEntityTypeConfiguration<MinistrySector>
    {
        public void Configure(EntityTypeBuilder<MinistrySector> builder)
        {
            builder.ToTable("MinistrySectors", "Lookups");

            builder.Property(m => m.Id)
                .ValueGeneratedNever();

            builder.Property(m => m.Name)
                .HasComment("The name of the Ministry Sector")
                .HasMaxLength(100)
                .IsRequired();
        }
    }
}
