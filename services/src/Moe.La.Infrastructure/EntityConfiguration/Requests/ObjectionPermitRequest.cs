using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    public class ObjectionPermitRequestConfiguration : IEntityTypeConfiguration<ObjectionPermitRequest>
    {
        public void Configure(EntityTypeBuilder<ObjectionPermitRequest> builder)
        {
            builder.ToTable("ObjectionPermitRequests", "Request");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.SuggestedOpinon)
                .HasComment("The suggested researcher opinion")
                .IsRequired();

            builder.Property(m => m.Note)
                .HasMaxLength(4000)
                .IsRequired();

            builder.HasOne(m => m.Request)
                .WithOne()
                .HasForeignKey<ObjectionPermitRequest>(r => r.Id)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.Case)
                .WithMany()
                .HasForeignKey(m => m.CaseId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
