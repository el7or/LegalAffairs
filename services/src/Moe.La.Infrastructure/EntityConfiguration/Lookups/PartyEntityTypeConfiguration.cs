using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    public class PartyEntityTypeConfiguration : BaseEntityConfiguration<PartyEntityType, int>
    {
        public override void Configure(EntityTypeBuilder<PartyEntityType> builder)
        {
            builder.ToTable("PartyEntityTypes", "Lookups");

            builder.Property(m => m.Id)
                .ValueGeneratedNever();

            builder.Property(m => m.Name)
                .HasComment("The name of the party entity type")
                .HasMaxLength(50)
                .IsRequired();

            base.Configure(builder);
        }
    }
}
