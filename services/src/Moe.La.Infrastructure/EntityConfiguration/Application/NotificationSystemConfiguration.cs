using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    public class NotificationSystemConfiguration : IEntityTypeConfiguration<NotificationSystem>
    {
        public void Configure(EntityTypeBuilder<NotificationSystem> builder)
        {
            builder.ToTable("NotificationSystem", "Application");

            builder.HasKey(m => new { m.NotificationId, m.UserId });

            builder.Property(m => m.NotificationId)
                .HasComment("The notification send to")
                .IsRequired();

            builder.Property(m => m.UserId)
                .HasComment("The user notification send to")
                .IsRequired();

            builder.HasOne(n => n.Notification)
                .WithMany(nn => nn.Systems)
                .HasForeignKey(n => n.NotificationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(m => m.Type)
                .HasComment("The type of the notification")
                .HasMaxLength(2)
                .IsRequired();

            builder.Property(m => m.URL)
                .HasComment("The URL value of the notification")
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property(m => m.IsRead)
                .HasComment("Used for the notification read or not")
                .HasDefaultValue(false)
                .IsRequired();

            builder.HasOne(n => n.User)
                .WithMany()
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
