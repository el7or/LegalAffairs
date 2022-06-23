using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    class InvestigationConfiguration : BaseEntityConfiguration<Investigation, int>
    {
        public override void Configure(EntityTypeBuilder<Investigation> builder)
        {
            builder.ToTable("Investigations", "Investigation");

            builder.Property(m => m.StartDate)
               .HasComment("Investigation date")
               .IsRequired();

            builder.Property(m => m.InvestigationNumber)
               .IsRequired();

            builder.Property(m => m.InvestigationStatus)
               .IsRequired();

            builder.Property(m => m.IsHasCriminalSide)
                .HasDefaultValue(false)
               .IsRequired();

            builder.Property(m => m.Subject)
                .HasMaxLength(2000)
               .IsRequired();

            builder.HasOne(m => m.CreatedByUser)
                .WithMany()
                .HasForeignKey(m => m.CreatedBy)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.UpdatedByUser)
                .WithMany()
                .HasForeignKey(m => m.UpdatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.CreatedByUser)
                .WithMany()
                .HasForeignKey(m => m.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.Investigator)
                .WithMany()
                .HasForeignKey(m => m.InvestigatorId)
                .OnDelete(DeleteBehavior.Restrict);


            builder.HasMany(m => m.InvestigationRecords)
                .WithOne(mm => mm.Investigation)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);


            base.Configure(builder);
        }
    }
}
