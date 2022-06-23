using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    public class AppUserClaimConfiguration : IEntityTypeConfiguration<AppUserClaim>
    {
        public void Configure(EntityTypeBuilder<AppUserClaim> builder)
        {
            builder.ToTable("UserClaims", "Application");

            builder.HasOne(m => m.User)
                .WithMany(m => m.UserClaims)
                .HasForeignKey(m => m.UserId)
                .IsRequired();
        }
    }
}
