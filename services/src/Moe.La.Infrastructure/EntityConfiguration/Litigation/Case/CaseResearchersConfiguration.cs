using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    public class CaseResearchersConfiguration : BaseEntityConfiguration<CaseResearcher, int>
    {
        public override void Configure(EntityTypeBuilder<CaseResearcher> builder)
        {
            builder.ToTable("CaseResearchers", "Case");

            builder.Property(m => m.Note)
                .HasMaxLength(400);

            builder.HasOne(m => m.Case)
                .WithMany(m => m.Researchers)
                .HasForeignKey(m => m.CaseId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.Researcher)
                .WithMany()
                .HasForeignKey(m => m.ResearcherId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.CreatedByUser)
                .WithMany()
                .HasForeignKey(m => m.CreatedBy)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            base.Configure(builder);
        }
    }
}
