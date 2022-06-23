using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    class HearingConfiguration : BaseEntityConfiguration<Hearing, int>
    {
        public override void Configure(EntityTypeBuilder<Hearing> builder)
        {
            builder.ToTable("Hearings", "Hearing");

            builder.Property(m => m.CaseId)
                .HasComment("The case that the hearing belongs to")
                .IsRequired();

            builder.Property(m => m.Status)
                .HasComment("The status of the hearing")
                .HasMaxLength(2)
                .IsRequired();

            builder.Property(m => m.Type)
               .HasComment("The type of the hearing")
               .HasMaxLength(2)
               .IsRequired();

            builder.Property(m => m.HearingNumber)
               .HasComment("The number of the hearing")
               .HasMaxLength(50)
               .IsRequired();

            builder.Property(m => m.CircleNumber)
               .HasComment("The circle no of the hearing")
               .HasMaxLength(64);

            builder.Property(m => m.HearingDate)
                .HasComment("The hearing date")
                .IsRequired();

            builder.Property(m => m.HearingTime)
                .HasComment("The hearing time")
                .HasMaxLength(50)
                .IsRequired(false);

            builder.Property(m => m.Summary)
               .HasComment("The hearing summary")
               .HasMaxLength(512)
               .IsRequired(false);

            builder.Property(m => m.SessionMinutes)
              .HasComment("The hearing session minutes")
              .HasMaxLength(2000)
              .IsRequired(false);

            builder.Property(m => m.ClosingReport)
                .HasComment("The hearing closing report")
                .HasMaxLength(512)
                .IsRequired(false);

            builder.Property(m => m.HearingDesc)
                .HasMaxLength(512);

            builder.Property(m => m.Motions)
                .HasMaxLength(1000);

            builder.HasOne(h => h.AssignedTo)
                .WithMany()
                .HasForeignKey(h => h.AssignedToId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(h => h.Case)
                .WithMany(h => h.Hearings)
                .HasForeignKey(h => h.Case‏Id‏)
                .IsRequired()
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
