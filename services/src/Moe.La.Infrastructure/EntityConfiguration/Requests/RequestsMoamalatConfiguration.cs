using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    public class RequestsMoamalatConfiguration : BaseEntityConfiguration<RequestsMoamalat, int>
    {
        public override void Configure(EntityTypeBuilder<RequestsMoamalat> builder)
        {
            builder.ToTable("RequestsMoamalat", "Moamalat");

            builder.Property(m => m.RequestId)
                .IsRequired()
                .HasComment("The request id");

            builder.Property(m => m.MoamalatId)
                .IsRequired()
                .HasComment("The moamalat id");

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
