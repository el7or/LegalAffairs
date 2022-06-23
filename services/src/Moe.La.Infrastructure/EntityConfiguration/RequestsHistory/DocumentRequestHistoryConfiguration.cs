using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    public class DocumentRequestHistoryConfiguration : IEntityTypeConfiguration<CaseSupportingDocumentRequestHistory>
    {
        public void Configure(EntityTypeBuilder<CaseSupportingDocumentRequestHistory> builder)
        {
            builder.ToTable("DocumentRequestHistory", "Request");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.IsDeleted)
                .HasComment("Used for the logical delete")
                .HasDefaultValue(false)
                .IsRequired();

            builder.HasOne(m => m.ConsigneeDepartment)
                .WithMany()
                .HasForeignKey(m => m.ConsigneeDepartmentId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.Hearing)
                .WithMany()
                .HasForeignKey(m => m.HearingId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.Request)
                .WithOne()
                .HasForeignKey<CaseSupportingDocumentRequestHistory>(r => r.Id)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.Parent)
                .WithMany()
                .HasForeignKey(r => r.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.CaseSupportingDocumentRequest)
               .WithMany(r => r.History)
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
