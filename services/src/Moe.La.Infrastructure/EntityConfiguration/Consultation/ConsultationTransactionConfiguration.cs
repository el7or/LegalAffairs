using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    public class ConsultationTransactionConfiguration : IEntityTypeConfiguration<ConsultationTransaction>
    {
        public void Configure(EntityTypeBuilder<ConsultationTransaction> builder)
        {
            builder.ToTable("ConsultationTransactions", "Consultation");

            builder.Property(m => m.Note)
                .HasComment("The note of the consultation transaction")
                .HasMaxLength(100)
                .IsRequired();

            builder.HasOne(m => m.Consultation)
                .WithMany(m => m.ConsultationTransactions)
                .HasForeignKey(m => m.ConsultationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.CreatedByUser)
             .WithMany()
             .HasForeignKey(m => m.CreatedBy)
             .IsRequired()
             .OnDelete(DeleteBehavior.Restrict);
        }
    }
}