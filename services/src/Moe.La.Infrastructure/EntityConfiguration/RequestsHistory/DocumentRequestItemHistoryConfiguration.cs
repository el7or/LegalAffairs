using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    public class DocumentRequestItemHistoryConfiguration : IEntityTypeConfiguration<CaseSupportingDocumentRequestItemHistory>
    {
        public void Configure(EntityTypeBuilder<CaseSupportingDocumentRequestItemHistory> builder)
        {
            builder.ToTable("DocumentRequestItemHistory", "Request");

            builder.HasKey(m => m.Id);

            builder.Property(d => d.Name)
                .HasComment("The document request item name")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(m => m.IsDeleted)
                .HasComment("Used for the logical delete")
                .HasDefaultValue(false)
                .IsRequired();

            builder.HasOne(d => d.CaseSupportingDocumentRequest)
                .WithMany(r => r.Documents)
                .HasForeignKey(m => m.CaseSupportingDocumentRequestId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.Case)
                .WithMany()
                .HasForeignKey(m => m.CaseId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasQueryFilter(m => !m.IsDeleted);
        }
    }
}
