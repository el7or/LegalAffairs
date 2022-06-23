using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    public class LegalMemoNoteConfiguration : BaseEntityConfiguration<LegalMemoNote, int>
    {
        public override void Configure(EntityTypeBuilder<LegalMemoNote> builder)
        {
            builder.ToTable("LegalMemoNotes", "Memo");

            builder.Property(m => m.ReviewNumber)
                .HasComment("The revision number")
                .IsRequired();

            builder.Property(m => m.Text)
                .HasMaxLength(2000)
                .HasComment("The text of the LegalMemoNote")
                .IsRequired();

            builder.Property(m => m.IsClosed)
                .HasComment("Determine that the note is no longer editable")
                .HasDefaultValue(false)
                .IsRequired();

            builder.Property(m => m.IsDeleted)
                .HasComment("Used for the logical delete")
                .HasDefaultValue(false)
                .IsRequired();

            builder.HasOne(m => m.LegalMemo)
                .WithMany()
                .HasForeignKey(m => m.LegalMemoId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.LegalBoard)
                .WithMany()
                .HasForeignKey(m => m.LegalBoardId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.CreatedByUser)
                .WithMany()
                .HasForeignKey(m => m.CreatedBy)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            base.Configure(builder);
        }
    }
}
