using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    public class LegalBoardMemberConfiguration : BaseEntityConfiguration<LegalBoardMember, int>
    {
        public override void Configure(EntityTypeBuilder<LegalBoardMember> builder)
        {
            builder.ToTable("LegalBoardMembers", "Memo");

            builder.HasOne(m => m.LegalBoard)
               .WithMany(m => m.LegalBoardMembers)
               .HasForeignKey(m => m.LegalBoardId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.User)
                .WithMany()
                .HasForeignKey(m => m.UserId)
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
