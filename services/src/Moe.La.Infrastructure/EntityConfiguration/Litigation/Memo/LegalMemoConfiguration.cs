using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    public class LegalMemoConfiguration : BaseEntityConfiguration<LegalMemo, int>
    {
        public override void Configure(EntityTypeBuilder<LegalMemo> builder)
        {
            builder.ToTable("LegalMemos", "Memo");

            builder.Property(m => m.Name)
                .HasComment("The name of the LegalMemo")
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(m => m.Status)
                .HasComment("The status of the LegalMemo")
                .HasMaxLength(2)
                .IsRequired();

            builder.Property(m => m.Text)
                .HasComment("The text of the LegalMemo")
                .IsRequired();

            builder.Property(m => m.InitialCaseId)
                .HasComment("The initial case of the LegalMemo")
                .IsRequired();

            //builder.HasOne(m => m.ApprovedBy)
            //   .WithMany()
            //   .HasForeignKey(m => m.ApprovedById)
            //   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.CreatedByUser)
                .WithMany()
                .HasForeignKey(m => m.CreatedBy)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.UpdatedByUser)
                .WithMany()
                .HasForeignKey(m => m.UpdatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(e => e.Name).IsUnique();

            base.Configure(builder);
        }
    }
}
