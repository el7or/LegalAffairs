using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    public class ConsultationSupportingDocumentConfiguration : IEntityTypeConfiguration<ConsultationSupportingDocumentRequest>
    {
        public void Configure(EntityTypeBuilder<ConsultationSupportingDocumentRequest> builder)
        {
            builder.ToTable("ConsultationSupportingDocuments", "Request");

            builder.HasKey(m => new { m.RequestId, m.ConsultationId });

            builder.Property(m => m.IsDeleted)
                .HasComment("Used for the logical delete")
                .HasDefaultValue(false)
                .IsRequired();

            builder.HasOne(m => m.Consultation)
                .WithMany(h => h.ConsultationSupportingDocuments)
                .HasForeignKey(m => m.ConsultationId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.Request)
                .WithMany(m => m.ConsultationSupportingDocuments)
                .HasForeignKey(r => r.RequestId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);


            builder.HasQueryFilter(m => !m.IsDeleted);
        }
    }
}
