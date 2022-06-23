using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration.Record
{
    public class InvestigationRecordInvestigatorConfiguration : BaseEntityConfiguration<InvestigationRecordInvestigator, int>
    {
        public override void Configure(EntityTypeBuilder<InvestigationRecordInvestigator> builder)
        {
            builder.ToTable("InvestigationRecordInvestigators", "Investigation");

            builder.HasOne(m => m.InvestigationRecord)
                .WithMany(m => m.InvestigationRecordInvestigators)
                .HasForeignKey(m => m.InvestigationRecordId);
            //.OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.Investigator)
                .WithMany()
                .HasForeignKey(m => m.InvestigatorId)
                .OnDelete(DeleteBehavior.Restrict);

            base.Configure(builder);
        }
    }
}
