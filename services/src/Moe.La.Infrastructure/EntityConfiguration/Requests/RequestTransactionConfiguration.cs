using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    class RequestTransactionConfiguration : BaseEntityConfiguration<RequestTransaction, int>
    {
        public override void Configure(EntityTypeBuilder<RequestTransaction> builder)
        {
            builder.ToTable("RequestTransactions", "Request");
            builder.Property(m => m.RequestId)
               .HasComment("The Id of the request")
               .IsRequired();

            builder.Property(m => m.RequestStatus)
                .HasComment("The status of the request")
                .IsRequired();

            builder.Property(m => m.CreatedBy)
                .HasComment("The Id of the user who did the transaction")
                .IsRequired();

            builder.Property(m => m.Description)
                .HasComment("The description of the transaction")
                .IsRequired();

            builder.Property(m => m.CreatedOn)
                .HasComment("The date and time of the transaction");

            builder.HasOne(m => m.Request)
               .WithMany(r => r.RequestTransactions)
               .HasForeignKey(m => m.RequestId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.CreatedByUser)
                .WithMany()
                .HasForeignKey(d => d.CreatedBy)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
