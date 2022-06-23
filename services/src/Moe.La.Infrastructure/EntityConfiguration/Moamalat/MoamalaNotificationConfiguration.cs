using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration.Moamalat
{
    public class MoamalaNotificationConfiguration : IEntityTypeConfiguration<MoamalaNotification>
    {
        public void Configure(EntityTypeBuilder<MoamalaNotification> builder)
        {
            builder.ToTable("MoamalatNotifications", "Moamalat");

            builder.HasOne(m => m.Moamala)
               .WithMany(c => c.MoamalaNotifications)
               .HasForeignKey(m => m.MoamalaId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.User)
                .WithMany()
                .HasForeignKey(m => m.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(m => m.IsRead)
                .HasDefaultValue(false)
                .IsRequired();
        }
    }
}
