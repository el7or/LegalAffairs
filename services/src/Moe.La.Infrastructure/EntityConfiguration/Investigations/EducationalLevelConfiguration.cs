using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    public class EducationalLevelConfiguration : BaseEntityConfiguration<EducationalLevel, int>
    {
        public override void Configure(EntityTypeBuilder<EducationalLevel> builder)
        {
            builder.ToTable("EducationalLevels", "Investigation");

            builder.HasOne(m => m.InvestigationRecordParty)
                .WithMany(m => m.EducationalLevels)
                .HasForeignKey(m => m.InvestigationRecordPartyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(m => m.EducationLevel)
                .HasComment("EducationLevel")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(m => m.ResidenceAddress)
                .HasComment("ResidenceAddress")
                .HasMaxLength(100)
                .IsRequired();

            base.Configure(builder);
        }
    }
}
