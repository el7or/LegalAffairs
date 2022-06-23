using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;
using System;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    class AttachmentConfiguration : BaseEntityConfiguration<Attachment, Guid>
    {
        public override void Configure(EntityTypeBuilder<Attachment> builder)
        {
            builder.ToTable("Attachments", "Application");

            builder.HasOne(u => u.AttachmentType)
             .WithMany()
             .HasForeignKey(u => u.AttachmentTypeId)
             .OnDelete(DeleteBehavior.Restrict);

            base.Configure(builder);

        }
    }
}
