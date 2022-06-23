using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    public class ConsultationMoamalatConfiguration : IEntityTypeConfiguration<ConsultationMoamalat>
    {
        public void Configure(EntityTypeBuilder<ConsultationMoamalat> builder)
        {
            builder.ToTable("ConsultationMoamalat", "Consultation");

            builder.HasKey(m => new { m.ConsultationId, m.MoamalaId });

            builder.HasOne(m => m.Consultation)
             .WithMany(m => m.ConsultationMoamalat)
             .HasForeignKey(m => m.ConsultationId)
             .IsRequired()
             .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.Moamala)
             .WithMany(m => m.ConsultationMoamalat)
             .HasForeignKey(m => m.MoamalaId)
             .IsRequired()
             .OnDelete(DeleteBehavior.Restrict);
        }
    }
}