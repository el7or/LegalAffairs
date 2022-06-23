using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    public class MinistryDepartmentConfiguration : IEntityTypeConfiguration<MinistryDepartment>
    {
        public void Configure(EntityTypeBuilder<MinistryDepartment> builder)
        {
            builder.ToTable("MinistryDepartments", "Lookups");

            builder.Property(m => m.Id)
                .ValueGeneratedNever();

            builder.Property(m => m.Name)
                .HasComment("The name of the Ministry Department")
                .HasMaxLength(100)
                .IsRequired();

            builder.HasOne(m => m.MinistrySector)
                .WithMany(m => m.MinistryDepartments)
                .HasForeignKey(m => m.MinistrySectorId)
                .IsRequired();
        }
    }
}
