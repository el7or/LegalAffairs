using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration.Consultation
{
    public class ConsultationVisualConfiguration : IEntityTypeConfiguration<ConsultationVisual>
    {
        public void Configure(EntityTypeBuilder<ConsultationVisual> builder)
        {
            builder.ToTable("ConsultationVisuals", "Consultation");

            builder.Property(m => m.Material)
                .HasComment("The Text of the material")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(m => m.Visuals)
                .HasComment("The Text of the visuals")
                .HasMaxLength(2000)
                .IsRequired();

            builder.HasOne(m => m.Consultation)
                .WithMany(m => m.ConsultationVisuals)
                .HasForeignKey(m => m.ConsultationId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
