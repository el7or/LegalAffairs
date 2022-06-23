using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    public class RequestConfiguration : BaseEntityConfiguration<Request, int>
    {
        public override void Configure(EntityTypeBuilder<Request> builder)
        {
            builder.ToTable("Requests", "Request");

            builder.Property(m => m.RequestType)
                .HasComment("The request type")
                .IsRequired();

            builder.Property(m => m.RequestStatus)
                .HasComment("The request status during its lifetime")
                .IsRequired();

            builder.Property(m => m.SendingType)
                .HasComment("The request sending type")
                .IsRequired();

            builder.Property(m => m.Note)
               .HasComment("The request reason")
               .HasMaxLength(400);

            builder.HasOne(m => m.RelatedRequest)
                .WithMany()
                .HasForeignKey(r => r.RelatedRequestId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.Letter)
               .WithOne()
               .HasForeignKey<RequestLetter>(m => m.RequestId)
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
