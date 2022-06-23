using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    public class ExportCaseJudgmentRequestHistoryConfiguration : IEntityTypeConfiguration<ExportCaseJudgmentRequestHistory>
    {
        public void Configure(EntityTypeBuilder<ExportCaseJudgmentRequestHistory> builder)
        {
            builder.ToTable("ExportCaseJudgmentRequestHistory", "Request");

            builder.HasKey(m => m.Id);

            builder.HasOne(m => m.Request)
                .WithOne()
                .HasForeignKey<ExportCaseJudgmentRequestHistory>(r => r.Id)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.ExportCaseJudgmentRequest)
               .WithMany(r => r.History)
               .HasForeignKey(m => m.ExportCaseJudgmentRequestId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.Case)
                .WithMany()
                .HasForeignKey(m => m.CaseId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
