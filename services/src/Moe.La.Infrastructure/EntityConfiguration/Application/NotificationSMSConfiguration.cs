using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    class NotificationSMSConfiguration : IEntityTypeConfiguration<NotificationSMS>
    {
        public void Configure(EntityTypeBuilder<NotificationSMS> builder)
        {
            builder.ToTable("NotificationSMSs", "Application");

            builder.HasKey(m => new { m.UserId, m.NotificationId });

            builder.HasOne(m => m.Notification)
            .WithMany(n => n.SMSs)
            .HasForeignKey(m => m.NotificationId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.User)
            .WithMany()
            .HasForeignKey(m => m.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

            builder.Property(m => m.IsSent)
              .HasComment("Used for the notification sent or not")
              .HasDefaultValue(false)
              .IsRequired();

            builder.Property(m => m.AttemptsCount)
               .HasComment("Used for the notification attempts count")
               .HasDefaultValue(0)
               .IsRequired();
        }
    }
}
