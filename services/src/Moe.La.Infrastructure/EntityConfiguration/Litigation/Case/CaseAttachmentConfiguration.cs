using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    public class CaseAttachmentConfiguration : IEntityTypeConfiguration<CaseAttachment>
    {
        public void Configure(EntityTypeBuilder<CaseAttachment> builder)
        {
            builder.ToTable("CaseAttachments", "Case");

            builder.HasOne(m => m.Case)
                .WithMany(m => m.Attachments)
                .HasForeignKey(m => m.CaseId)
                //.IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.Attachment)
               .WithOne()
               .HasForeignKey<CaseAttachment>(r => r.Id)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);



        }
    }
}
