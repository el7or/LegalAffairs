using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities.Litigation.BoardMeeting;

namespace Moe.La.Infrastructure.EntityConfiguration.Litigation.Memo
{
    public class BoardMeetingMemberConfiguration : BaseEntityConfiguration<BoardMeetingMember, int>
    {
        public override void Configure(EntityTypeBuilder<BoardMeetingMember> builder)
        {
            builder.ToTable("BoardMeetingMembers", "Memo");

            builder.HasOne(m => m.BoardMeeting)
                .WithMany(m => m.BoardMeetingMembers)
                .HasForeignKey(m => m.BoardMeetingId)
                .IsRequired();

            builder.HasOne(m => m.BoardMember)
                .WithMany()
                .HasForeignKey(m => m.BoardMemberId)
                .IsRequired();

            builder.HasOne(e => e.CreatedByUser)
                .WithMany()
                .HasForeignKey(d => d.CreatedBy)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            base.Configure(builder);
        }
    }
}
