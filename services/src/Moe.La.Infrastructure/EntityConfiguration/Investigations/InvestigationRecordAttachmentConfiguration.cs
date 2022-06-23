using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration.Record
{
    public class InvestigationRecordAttachmentConfiguration : IEntityTypeConfiguration<InvestigationRecordAttachment>
    {
        public void Configure(EntityTypeBuilder<InvestigationRecordAttachment> builder)
        {
            builder.ToTable("InvestigationRecordAttachments", "Investigation");

            builder.HasOne(m => m.Record)
                .WithMany(m => m.Attachments)
                .HasForeignKey(m => m.RecordId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.Attachment)
               .WithOne()
               .HasForeignKey<InvestigationRecordAttachment>(r => r.Id)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
