using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;
using Moe.La.Core.Entities.RequestsHistory;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    public class RequestHistoryConfiguration : BaseEntityConfiguration<RequestHistory, int>
    {
        public override void Configure(EntityTypeBuilder<RequestHistory> builder)
        {
            builder.ToTable("RequestHistory", "Request");

            builder.HasOne(m => m.RelatedRequest)
                .WithMany()
                .HasForeignKey(r => r.RelatedRequestId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.Letter)
             .WithOne()
             .HasForeignKey<RequestLetterHistory>(m => m.RequestId)
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
