using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    public class AddingLegalMemoToHearingRequestConfiguration : IEntityTypeConfiguration<AddingLegalMemoToHearingRequest>
    {
        public void Configure(EntityTypeBuilder<AddingLegalMemoToHearingRequest> builder)
        {
            builder.ToTable("AddingLegalMemoToHearingRequests", "Request");

            builder.HasOne(m => m.Hearing)
                .WithMany(m => m.HearingLegalMemoReviewRequests)
                .HasForeignKey(m => m.HearingId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.LegalMemo)
                .WithMany()
                .HasForeignKey(m => m.LegalMemoId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.Request)
               .WithOne()
               .HasForeignKey<AddingLegalMemoToHearingRequest>(r => r.Id)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(m => m.Case)
                .WithMany()
                .HasForeignKey(m => m.CaseId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
