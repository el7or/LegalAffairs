using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    public class ConsultationGroundsConfiguration : IEntityTypeConfiguration<ConsultationGrounds>
    {
        public void Configure(EntityTypeBuilder<ConsultationGrounds> builder)
        {
            builder.ToTable("ConsultationGrounds", "Consultation");

            builder.Property(m => m.Text)
                .HasComment("The Text of the Consultation Grounds")
                .HasMaxLength(100)
                .IsRequired();

            builder.HasOne(m => m.Consultation)
               .WithMany(m => m.ConsultationGrounds)
               .HasForeignKey(m => m.ConsultationId)
               .OnDelete(DeleteBehavior.Restrict);
        }
    }
}