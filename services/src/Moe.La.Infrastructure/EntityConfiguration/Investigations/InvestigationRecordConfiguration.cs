using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    class InvestigationRecordConfiguration : BaseEntityConfiguration<InvestigationRecord, int>
    {
        public override void Configure(EntityTypeBuilder<InvestigationRecord> builder)
        {
            builder.ToTable("InvestigationRecords", "Investigation");

            builder.Property(m => m.EndDate)
               .HasComment("record end time")
               .IsRequired();

            builder.Property(m => m.StartDate)
               .HasComment("record start time")
               .IsRequired();

            builder.Property(m => m.Visuals)
                .HasComment("record visuals")
                .HasMaxLength(2000);

            builder.Property(m => m.RecordStatus)
                .HasComment("record status")
                .IsRequired();

            builder.HasMany(m => m.InvestigationRecordParties)
                .WithOne()
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
