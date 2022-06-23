using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration.Record
{
    public class InvestigationRecordPartyConfiguration : BaseEntityConfiguration<InvestigationRecordParty, int>
    {
        public override void Configure(EntityTypeBuilder<InvestigationRecordParty> builder)
        {
            builder.ToTable("InvestigationRecordParties", "Investigation");

            builder.HasOne(m => m.InvestigationRecord)
                .WithMany(m => m.InvestigationRecordParties)
                .HasForeignKey(m => m.InvestigationRecordId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(m => m.PartyName)
                .HasComment("party name")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(m => m.InvestigationRecordPartyTypeId)
                .HasComment("The related investigation record party type ID.")
                .IsRequired();

            builder.HasOne(m => m.InvestigationRecordPartyType)
                .WithMany()
                .HasForeignKey(m => m.InvestigationRecordPartyTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            base.Configure(builder);
        }
    }
}
