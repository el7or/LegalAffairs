using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    public class RequestNoteConfiguration : BaseEntityConfiguration<RequestNote, int>
    {
        public override void Configure(EntityTypeBuilder<RequestNote> builder)
        {
            builder.ToTable("RequestNotes", "Request");

            builder.Property(m => m.Text)
                .HasMaxLength(2000)
                .HasComment("The text of the LegalMemoNote")
                .IsRequired();

            builder.Property(m => m.IsDeleted)
                .HasComment("Used for the logical delete")
                .HasDefaultValue(false)
                .IsRequired();

            builder.HasOne(m => m.Request)
                .WithMany()
                .HasForeignKey(m => m.RequestId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.Role)
                .WithMany()
                .HasForeignKey(m => m.RoleId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.CreatedByUser)
                .WithMany()
                .HasForeignKey(m => m.CreatedBy)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            base.Configure(builder);
        }
    }
}
