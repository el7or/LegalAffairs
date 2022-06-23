using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    class HearingUpdateConfiguration : BaseEntityConfiguration<HearingUpdate, int>
    {
        public override void Configure(EntityTypeBuilder<HearingUpdate> builder)
        {
            builder.ToTable("HearingUpdates", "Hearing");

            builder.Property(m => m.Text)
               .HasComment("The text of the update")
               .HasMaxLength(200)
               .IsRequired();

            builder.HasOne(m => m.CreatedByUser)
              .WithMany()
              .HasForeignKey(m => m.CreatedBy)
              .IsRequired()
              .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.Hearing)
              .WithMany()
              .HasForeignKey(m => m.HearingId)
              .IsRequired()
              .OnDelete(DeleteBehavior.Restrict);

            base.Configure(builder);
        }
    }
}
