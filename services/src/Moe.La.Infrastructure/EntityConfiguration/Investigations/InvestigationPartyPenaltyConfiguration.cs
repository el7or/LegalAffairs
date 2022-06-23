using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    public class InvestigationPartyPenaltyConfiguration : BaseEntityConfiguration<InvestigationPartyPenalty, int>
    {
        public override void Configure(EntityTypeBuilder<InvestigationPartyPenalty> builder)
        {
            builder.ToTable("InvestigationPartyPenalties", "Investigation");

            builder.HasOne(m => m.InvestigationRecordParty)
                .WithMany(m => m.InvestigationPartyPenalties)
                .HasForeignKey(m => m.InvestigationRecordPartyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(m => m.Penalty)
                .HasComment("Penalty")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(m => m.Reasons)
                .HasComment("Reasons")
                .HasMaxLength(400)
                .IsRequired();

            base.Configure(builder);
        }
    }
}
