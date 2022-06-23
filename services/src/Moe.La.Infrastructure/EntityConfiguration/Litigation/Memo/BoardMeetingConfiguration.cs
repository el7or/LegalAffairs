using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities.Litigation.BoardMeeting;

namespace Moe.La.Infrastructure.EntityConfiguration.Litigation.Memo
{
    public class BoardMeetingConfiguration : BaseEntityConfiguration<BoardMeeting, int>
    {
        public override void Configure(EntityTypeBuilder<BoardMeeting> builder)
        {
            builder.ToTable("BoardMeetings", "Memo");

            builder.HasOne(m => m.Memo)
                .WithMany(m => m.BoardMeetings)
                .HasForeignKey(m => m.MemoId)
                .IsRequired();

            builder.HasOne(m => m.Board)
                .WithMany(m => m.BoardMeetings)
                .HasForeignKey(m => m.BoardId)
                .IsRequired();

            builder.HasOne(e => e.CreatedByUser)
                .WithMany()
                .HasForeignKey(d => d.CreatedBy)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.UpdatedByUser)
                .WithMany()
                .HasForeignKey(d => d.UpdatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            base.Configure(builder);
        }
    }
}
