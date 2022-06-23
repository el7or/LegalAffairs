using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    public class PartyTypeConfiguration : BaseEntityConfiguration<PartyType, int>
    {
        public override void Configure(EntityTypeBuilder<PartyType> builder)
        {
            builder.ToTable("PartyTypes", "Lookups");

            builder.Property(m => m.Id)
                .ValueGeneratedNever();

            builder.Property(m => m.Name)
                .HasComment("The name of the party type")
                .HasMaxLength(50)
                .IsRequired();

            base.Configure(builder);
        }
    }
}
