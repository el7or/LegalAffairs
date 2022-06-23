using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    class MoamalaRaselConfiguration : BaseEntityConfiguration<MoamalaRasel, int>
    {
        public override void Configure(EntityTypeBuilder<MoamalaRasel> builder)
        {
            builder.ToTable("Moamalat", "Rasel");

            builder.Property(m => m.UnifiedNumber)
                .HasComment("The unified number links many moamalat with each other")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(m => m.CustomNumber)
                .HasComment("The custom number")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(m => m.PreviousItemNumber)
                .HasComment("The previous moamal number in the chain of moamal linked with the unified number")
                .IsRequired(false);

            builder.Property(m => m.Subject)
                .HasComment("The subject of the moamala")
                .HasMaxLength(2000)
                .IsRequired();

            builder.Property(m => m.Comments)
                .HasComment("The comments of the moamala")
                .HasMaxLength(2000)
                .IsRequired();

            builder.Property(m => m.UnifiedNumber)
                .HasComment("The UnifiedNo of the moamala")
                .HasMaxLength(100)
                .IsRequired();
        }
    }
}
