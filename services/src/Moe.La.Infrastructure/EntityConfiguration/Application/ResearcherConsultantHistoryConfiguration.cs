using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    public class ResearcherConsultantHistoryConfiguration : BaseEntityConfiguration<ResearcherConsultantHistory, int>
    {
        public override void Configure(EntityTypeBuilder<ResearcherConsultantHistory> builder)
        {
            builder.ToTable("ResearcherConsultantHistory", "Application");

            builder.Property(m => m.ConsultantId)
                .HasComment("The consultant id");

            builder.Property(m => m.ResearcherId)
                .HasComment("The reasearcher id");

            builder.HasOne(m => m.Researcher)
                .WithMany()
                .HasForeignKey(m => m.ResearcherId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.ResearcherConsultant)
                .WithMany()
                .HasForeignKey(m => m.ResearcherConsultantId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.CreatedByUser)
               .WithMany()
               .HasForeignKey(m => m.CreatedBy)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.UpdatedByUser)
                .WithMany()
                .HasForeignKey(m => m.UpdatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            base.Configure(builder);
        }
    }
}
