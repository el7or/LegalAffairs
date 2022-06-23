using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    public class ConsultationConfiguration : IEntityTypeConfiguration<Core.Entities.Consultation>
    {
        public void Configure(EntityTypeBuilder<Core.Entities.Consultation> builder)
        {
            builder.ToTable("Consultations", "Consultation");

            builder.Property(m => m.Subject)
                .HasComment("The subject of the consultation")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(m => m.ConsultationNumber)
                .HasComment("The number of the consultation")
                .HasMaxLength(100);

            builder.Property(m => m.ConsultationDate)
                .HasComment("The date of the consultation");

            builder.Property(m => m.LegalAnalysis)
              .HasComment("The legal analysis of the consultation")
              .HasMaxLength(2000)
              .IsRequired();

            builder.Property(m => m.ImportantElements)
                .HasComment("The important elements of the consultation")
                .HasMaxLength(100);

            builder.Property(m => m.DepartmentVision)
              .HasComment("The department vision of the consultation")
              .HasMaxLength(2000)
              .IsRequired(false);

            builder.HasOne(m => m.WorkItemType)
             .WithMany()
             .HasForeignKey(m => m.WorkItemTypeId)
             .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.SubWorkItemType)
             .WithMany()
             .HasForeignKey(m => m.SubWorkItemTypeId)
             .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.Branch)
             .WithMany()
             .HasForeignKey(m => m.BranchId)
             .IsRequired()
             .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.Department)
             .WithMany()
             .HasForeignKey(m => m.DepartmentId)
             .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.CreatedByUser)
             .WithMany()
             .HasForeignKey(m => m.CreatedBy)
             .IsRequired()
             .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.UpdatedUser)
             .WithMany()
             .HasForeignKey(m => m.UpdatedBy)
             .OnDelete(DeleteBehavior.Restrict);
        }
    }
}