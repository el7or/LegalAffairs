using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    class MoamalaConfiguration : IEntityTypeConfiguration<Moamala>
    {
        public void Configure(EntityTypeBuilder<Moamala> builder)
        {
            builder.ToTable("Moamalat", "Moamalat");

            builder.Property(m => m.Subject)
                .HasComment("The subject of the moamala")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(m => m.Description)
                .HasComment("The details of the moamala")
                .HasMaxLength(2000)
                .IsRequired();

            builder.Property(m => m.MoamalaNumber)
                .HasComment("The ReferenceNo of the moamala")
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(m => m.UnifiedNo)
                .HasComment("The UnifiedNo of the moamala")
                .HasMaxLength(20);
            //.IsRequired();  

            builder.Property(m => m.BranchId)
                .HasComment("The moamala general managment");
            //.IsRequired();

            builder.Property(m => m.IsManual)
                .HasComment("If case created manualy not by integrated systems")
                .IsRequired();

            builder.HasOne(m => m.SenderDepartment)
             .WithMany()
             .HasForeignKey(m => m.SenderDepartmentId)
             .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.Branch)
                .WithMany()
                .HasForeignKey(m => m.BranchId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.ReceiverDepartment)
             .WithMany()
             .HasForeignKey(m => m.ReceiverDepartmentId)
             .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.AssignedTo)
             .WithMany()
             .HasForeignKey(m => m.AssignedToId)
             .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.CreatedByUser)
              .WithMany()
              .HasForeignKey(m => m.CreatedBy)
              .IsRequired()
              .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
