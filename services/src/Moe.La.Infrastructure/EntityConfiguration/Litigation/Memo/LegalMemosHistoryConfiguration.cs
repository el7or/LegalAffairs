using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    public class LegalMemosHistoryConfiguration : BaseEntityConfiguration<LegalMemoHistory, int>
    {
        public override void Configure(EntityTypeBuilder<LegalMemoHistory> builder)
        {
            builder.ToTable("LegalMemosHistory", "Memo");

            builder.Property(m => m.LegalMemoId)
                .HasComment("The legal memo");

            builder.Property(m => m.Name)
                .HasComment("The name of the LegalMemo")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(m => m.Status)
                .HasComment("The status of the LegalMemo")
                .HasMaxLength(2)
                .IsRequired();

            builder.HasOne(m => m.LegalMemo)
                .WithMany()
                .HasForeignKey(m => m.LegalMemoId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(m => m.InitialCaseId)
               .HasComment("The initial case of the LegalMemo")
               .IsRequired();

            builder.HasOne(m => m.CreatedByUser)
               .WithMany()
               .HasForeignKey(m => m.CreatedBy)
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
