using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    public class MoamalaAttachmentConfiguration : IEntityTypeConfiguration<MoamalaAttachment>
    {
        public void Configure(EntityTypeBuilder<MoamalaAttachment> builder)
        {
            builder.ToTable("MoamalaAttachments", "Moamalat");

            builder.HasOne(m => m.Moamala)
                .WithMany(m => m.Attachments)
                .HasForeignKey(m => m.MoamalaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.Attachment)
                .WithOne()
                .HasForeignKey<MoamalaAttachment>(r => r.Id)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
