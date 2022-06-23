using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration.Record
{
    public class InvestigationRecordAttendantConfiguration : BaseEntityConfiguration<InvestigationRecordAttendant, int>
    {
        public override void Configure(EntityTypeBuilder<InvestigationRecordAttendant> builder)
        {
            builder.ToTable("InvestigationRecordAttendants", "Investigation");

            builder.HasOne(m => m.InvestigationRecord)
                .WithMany(m => m.Attendants)
                .HasForeignKey(m => m.InvestigationRecordId);
            //.OnDelete(DeleteBehavior.Restrict); 

            builder.Property(m => m.IdentityNumber)
                .HasComment("Attendant IdentityNumber")
                .HasMaxLength(12)
                .IsRequired();

            builder.Property(m => m.RepresentativeOfId)
                .HasComment("Attendant RepresentativeOf.")
                .IsRequired();

            builder.HasOne(m => m.RepresentativeOf)
               .WithMany()
               .HasForeignKey(m => m.RepresentativeOfId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.Property(m => m.WorkLocation)
                .HasComment("Attendant WorkLocation.")
                .HasMaxLength(200)
                .IsRequired();

            base.Configure(builder);
        }
    }
}
