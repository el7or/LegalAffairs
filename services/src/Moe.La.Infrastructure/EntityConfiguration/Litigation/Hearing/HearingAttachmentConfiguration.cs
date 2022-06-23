using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    public class HearingAttachmentConfiguration : IEntityTypeConfiguration<HearingAttachment>
    {
        public void Configure(EntityTypeBuilder<HearingAttachment> builder)
        {
            builder.ToTable("HearingAttachments", "Hearing");

            builder.HasOne(m => m.Hearing)
                .WithMany(m => m.Attachments)
                .HasForeignKey(m => m.HearingId)
                //.IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.Attachment)
                .WithOne()
                .HasForeignKey<HearingAttachment>(r => r.Id)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
