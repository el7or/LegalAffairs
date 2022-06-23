using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    public class CaseRuleConfiguration : BaseEntityConfiguration<CaseRule, int>
    {
        public override void Configure(EntityTypeBuilder<CaseRule> builder)
        {
            builder.ToTable("CaseRules", "Case");

            builder.Property(m => m.JudgementText)
                .HasComment("The Judgement Text")
                .HasMaxLength(2000);

            builder.Property(m => m.JudgmentBrief)
                .HasComment("The Judgement Brief")
                .HasMaxLength(500);

            builder.Property(m => m.Feedback)
                .HasComment("The Feed Back")
                .HasMaxLength(500);

            builder.Property(m => m.FinalConclusions)
                .HasComment("The Final Conclusions")
                .HasMaxLength(500);

            builder.Property(m => m.JudgmentReasons)
                .HasComment("The Judgment Reasons")
                .HasMaxLength(500);

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
