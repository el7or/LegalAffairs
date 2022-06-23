using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    public class DocumentRequestConfiguration : IEntityTypeConfiguration<CaseSupportingDocumentRequest>
    {
        public void Configure(EntityTypeBuilder<CaseSupportingDocumentRequest> builder)
        {
            builder.ToTable("DocumentRequests", "Request");

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
                .WithMany(h => h.CaseSupportingDocumentRequests)
                .HasForeignKey(m => m.HearingId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.Request)
                .WithOne()
                .HasForeignKey<CaseSupportingDocumentRequest>(r => r.Id)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.Parent)
                .WithMany()
                .HasForeignKey(r => r.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasQueryFilter(m => !m.IsDeleted);
        }
    }
}
