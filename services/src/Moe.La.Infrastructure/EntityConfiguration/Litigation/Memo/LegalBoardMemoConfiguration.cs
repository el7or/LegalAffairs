using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    public class LegalBoardMemoConfiguration : BaseEntityConfiguration<LegalBoardMemo, int>
    {
        public override void Configure(EntityTypeBuilder<LegalBoardMemo> builder)
        {
            builder.ToTable("LegalBoardsMemos", "Memo");

            builder.HasOne(m => m.LegalMemo)
                .WithMany(m => m.LegalBoardMemos)
                .HasForeignKey(m => m.LegalMemoId)
                .IsRequired();

            builder.HasOne(m => m.LegalBoard)
                .WithMany(m => m.LegalBoardMemos)
                .HasForeignKey(m => m.LegalBoardId)
                .IsRequired();

            builder.HasOne(e => e.CreatedByUser)
                .WithMany()
                .HasForeignKey(d => d.CreatedBy)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.UpdatedByUser)
                .WithMany()
                .HasForeignKey(m => m.UpdatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            base.Configure(builder);
        }
    }
}
