using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    public class GovernmentOrganizationConfiguration : IEntityTypeConfiguration<GovernmentOrganization>
    {
        public void Configure(EntityTypeBuilder<GovernmentOrganization> builder)
        {
            builder.ToTable("GovernmentOrganizations", "Lookups");

            builder.Property(m => m.Id)
                .ValueGeneratedNever();

            builder.Property(m => m.Name)
                .HasComment("The name of the GovernmentOrganization")
                .HasMaxLength(100)
                .IsUnicode()
                .IsRequired();

            builder.Property(m => m.Phone)
                .HasComment("The phone of the GovernmentOrganization")
                .HasMaxLength(10)
                .IsRequired(false)
                .IsUnicode();

            builder.Property(m => m.Email)
                .HasComment("The email of the GovernmentOrganization")
                .HasMaxLength(50)
                .IsRequired(false)
                .IsUnicode();
        }
    }
}
