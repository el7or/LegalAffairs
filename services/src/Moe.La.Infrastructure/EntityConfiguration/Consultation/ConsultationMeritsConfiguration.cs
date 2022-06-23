using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    public class ConsultationMeritsConfiguration : IEntityTypeConfiguration<ConsultationMerits>
    {
        public void Configure(EntityTypeBuilder<ConsultationMerits> builder)
        {
            builder.ToTable("ConsultationMerits", "Consultation");

            builder.Property(m => m.Text)
                .HasComment("The Text of the merits")
                .HasMaxLength(100)
                .IsRequired();

            builder.HasOne(m => m.Consultation)
                .WithMany(m => m.ConsultationMerits)
                .HasForeignKey(m => m.ConsultationId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}