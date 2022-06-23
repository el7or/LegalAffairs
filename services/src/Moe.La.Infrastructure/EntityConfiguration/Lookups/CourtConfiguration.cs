using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    class CourtConfiguration : IEntityTypeConfiguration<Court>
    {
        public void Configure(EntityTypeBuilder<Court> builder)
        {
            builder.ToTable("Courts", "Lookups");

            builder.Property(m => m.Id)
                .ValueGeneratedNever();

            builder.Property(m => m.Name)
                .HasComment("The name of the court")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(m => m.LitigationType)
                .HasComment("The Litigation type that the court belongs to");

            builder.Property(m => m.Code)
                .HasComment("The code of the court that comes from the integration API")
                .HasMaxLength(10);
        }
    }
}
