using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    public class EvaluationsConfiuration : BaseEntityConfiguration<Evaluation, int>
    {
        public override void Configure(EntityTypeBuilder<Evaluation> builder)
        {
            builder.ToTable("Evaluations", "Investigation");

            builder.HasOne(m => m.InvestigationRecordParty)
                .WithMany(m => m.Evaluations)
                .HasForeignKey(m => m.InvestigationRecordPartyId)
                .OnDelete(DeleteBehavior.Restrict);

            base.Configure(builder);
        }
    }
}
