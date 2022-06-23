using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    public class ExportCaseJudgmentRequestConfiguration : IEntityTypeConfiguration<ExportCaseJudgmentRequest>
    {
        public void Configure(EntityTypeBuilder<ExportCaseJudgmentRequest> builder)
        {
            builder.ToTable("ExportCaseJudgmentRequests", "Request");

            builder.HasKey(m => m.Id);


            builder.HasOne(m => m.Request)
                .WithOne()
                .HasForeignKey<ExportCaseJudgmentRequest>(r => r.Id)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(m => m.Case)
                .WithMany()
                .HasForeignKey(m => m.CaseId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
