using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    public class HearingUpdateAttachmentConfiguration : IEntityTypeConfiguration<HearingUpdateAttachment>
    {
        public void Configure(EntityTypeBuilder<HearingUpdateAttachment> builder)
        {
            builder.ToTable("HearingUpdateAttachments", "Hearing");

            builder.HasOne(m => m.HearingUpdate)
                .WithMany(m => m.Attachments)
                .HasForeignKey(m => m.HearingUpdateId)
                //.IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.Attachment)
                .WithOne()
                .HasForeignKey<HearingUpdateAttachment>(r => r.Id)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
