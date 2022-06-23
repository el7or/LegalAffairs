using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    public class CasePartyConfiguration : BaseEntityConfiguration<CaseParty, int>
    {
        public override void Configure(EntityTypeBuilder<CaseParty> builder)
        {
            builder.ToTable("CaseParties", "Case");


            builder.Property(m => m.CaseId)
                .HasComment("The source of the case")
                .IsRequired();

            builder.Property(m => m.PartyId)
                .HasComment("The source of the party")
                .IsRequired();

            builder.Property(m => m.PartyStatus);

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
