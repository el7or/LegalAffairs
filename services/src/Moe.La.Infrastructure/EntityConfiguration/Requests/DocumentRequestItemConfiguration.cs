using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    public class DocumentRequestItemConfiguration : IEntityTypeConfiguration<CaseSupportingDocumentRequestItem>
    {
        public void Configure(EntityTypeBuilder<CaseSupportingDocumentRequestItem> builder)
        {
            builder.ToTable("DocumentRequestItems", "Request");

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

            builder.HasQueryFilter(m => !m.IsDeleted);
        }
    }
}
