using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    public class InvestigationRecordPartyTypeConfiguration : IEntityTypeConfiguration<InvestigationRecordPartyType>
    {
        public void Configure(EntityTypeBuilder<InvestigationRecordPartyType> builder)
        {
            builder.ToTable("InvestigationRecordPartyTypes", "Lookups");

            builder.Property(m => m.Id)
                .ValueGeneratedNever();

            builder.Property(m => m.Name)
                .HasComment("The name of the Investigation Record Party Type")
                .HasMaxLength(50)
                .IsRequired();
        }
    }
}
