using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    public class CaseRuleAttachmentConfiguration : IEntityTypeConfiguration<CaseRuleAttachment>
    {
        public void Configure(EntityTypeBuilder<CaseRuleAttachment> builder)
        {
            builder.ToTable("CaseRuleAttachments", "Case");

            builder.HasOne(m => m.CaseRule)
                .WithMany(m => m.Attachments)
                .HasForeignKey(m => m.CaseRuleId)
                //.IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.Attachment)
               .WithOne()
               .HasForeignKey<CaseRuleAttachment>(r => r.Id)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
