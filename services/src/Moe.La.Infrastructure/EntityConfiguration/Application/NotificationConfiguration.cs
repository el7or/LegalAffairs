using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    class NotificationConfiguration : BaseEntityConfiguration<Notification, int>
    {
        public override void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable("Notifications", "Application");

            builder.Property(m => m.Text)
                .HasComment("The Text value of the notification")
                .HasMaxLength(500)
                .IsRequired();


            base.Configure(builder);
        }
    }
}
