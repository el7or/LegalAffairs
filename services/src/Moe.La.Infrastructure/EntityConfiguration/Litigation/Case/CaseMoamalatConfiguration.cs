using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    public class CaseMoamalatConfiguration : IEntityTypeConfiguration<CaseMoamala>
    {
        public void Configure(EntityTypeBuilder<CaseMoamala> builder)
        {
            builder.ToTable("CaseMoamalat", "Case");

            builder.HasKey(m => new { m.CaseId, m.MoamalaId });

            builder.Property(m => m.CreatedBy)
                .HasComment("The user who created it.")
                .IsRequired();

            builder.HasOne(m => m.Case)
                .WithMany(m => m.CaseMoamalat)
                .HasForeignKey(m => m.CaseId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.Moamala)
                .WithMany(m => m.CaseMoamalat)
                .HasForeignKey(m => m.MoamalaId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasQueryFilter(m => !m.IsDeleted);
        }
    }
}