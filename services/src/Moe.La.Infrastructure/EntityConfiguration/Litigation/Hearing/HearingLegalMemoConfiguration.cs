using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration
{

    public class HearingLegalMemoConfiguration : BaseEntityConfiguration<HearingLegalMemo, int>
    {
        public override void Configure(EntityTypeBuilder<HearingLegalMemo> builder)
        {
            builder.ToTable("HearingLegalMemos", "Hearing");

            builder.HasOne(m => m.Hearing)
                .WithMany(m => m.HearingLegalMemos)
                .HasForeignKey(m => m.HearingId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.LegalMemo)
                .WithMany()
                .HasForeignKey(m => m.LegalMemoId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.CreatedByUser)
                .WithMany()
                .HasForeignKey(d => d.CreatedBy)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            base.Configure(builder);
        }
    }
}
