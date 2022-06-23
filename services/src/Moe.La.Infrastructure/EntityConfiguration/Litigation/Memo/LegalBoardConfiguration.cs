using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    public class LegalBoardConfiguration : BaseEntityConfiguration<LegalBoard, int>
    {
        public override void Configure(EntityTypeBuilder<LegalBoard> builder)
        {
            builder.ToTable("LegalBoards", "Memo");

            builder.Property(m => m.Name)
                .HasComment("The name of the legal board")
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(m => m.LegalBoardTypeId)
                .HasComment("The legal board type")
                .IsRequired();

            builder.Property(m => m.StatusId)
                .HasComment("The legal board status")
                .IsRequired();

            builder.HasOne(e => e.CreatedByUser)
               .WithMany()
               .HasForeignKey(d => d.CreatedBy)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.UpdatedByUser)
                .WithMany()
                .HasForeignKey(d => d.UpdatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            base.Configure(builder);
        }
    }
}
