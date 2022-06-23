using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    public class PartyConfiguration : BaseEntityConfiguration<Party, int>
    {
        public override void Configure(EntityTypeBuilder<Party> builder)
        {
            builder.ToTable("Parties", "Case");

            builder.Property(m => m.PartyType)
                .HasComment("The party type that the party belongs to");

            builder.Property(m => m.IdentityTypeId)
                .HasComment("The identity type that the party belongs to");

            builder.Property(m => m.IdentityValue)
                .HasComment("The identity value of the party")
                .HasMaxLength(30);

            builder.Property(m => m.IdentitySource)
                .HasComment("The identity source of the party")
                .HasMaxLength(30);

            builder.Property(m => m.Mobile)
                .HasComment("The mobile of the party")
                .HasMaxLength(30);

            builder.Property(m => m.Email)
               .HasComment("The mobile of the party")
               .HasMaxLength(50);

            builder.Property(m => m.Street)
                .HasComment("The street of the party address")
                .HasMaxLength(20);

            builder.Property(m => m.BuidlingNumber)
                .HasComment("The buidlign number of the party address")
                .HasMaxLength(10);

            builder.Property(m => m.PostalCode)
                .HasComment("The postal code of the party address")
                .HasMaxLength(8);

            builder.Property(m => m.AddressDetails)
                .HasComment("The details of the party address")
                .HasMaxLength(50);

            builder.Property(m => m.TelephoneNumber)
                .HasComment("The telephone number of the party address")
                .HasMaxLength(15);

            builder.HasOne(p => p.Province)
                .WithMany()
                .HasForeignKey(p => p.ProvinceId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.City)
                .WithMany()
                .HasForeignKey(p => p.CityId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.IdentityType)
                .WithMany(i => i.Parties)
                .HasForeignKey(p => p.IdentityTypeId)
                //.IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

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
