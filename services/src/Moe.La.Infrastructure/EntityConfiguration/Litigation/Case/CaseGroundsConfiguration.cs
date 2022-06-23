using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    public class CaseGroundsConfiguration : BaseEntityConfiguration<CaseGrounds, int>
    {
        public override void Configure(EntityTypeBuilder<CaseGrounds> builder)
        {
            builder.ToTable("CaseGrounds", "Case");

            builder.HasKey(m => new { m.CaseId, m.Text });

            builder.Property(m => m.CaseId)
                .HasComment("The case id");

            builder.HasOne(m => m.Case)
                .WithMany(m => m.CaseGrounds)
                .HasForeignKey(m => m.CaseId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(m => m.Text)
                .HasComment("The text the case grounds")
                .HasMaxLength(400)
                .IsRequired();

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
