using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    class FieldMissionTypeConfiguration : BaseEntityConfiguration<FieldMissionType, int>
    {
        public override void Configure(EntityTypeBuilder<FieldMissionType> builder)
        {
            builder.ToTable("FieldMissionTypes", "Lookups");

            builder.Property(m => m.Id)
                .ValueGeneratedNever();

            builder.Property(m => m.Name)
                .HasComment("The name of the field mission type")
                .HasMaxLength(50)
                .IsRequired();

            base.Configure(builder);
        }
    }
}
