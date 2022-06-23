using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    class AppUserRefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("RefreshTokens", "Application");

            builder.HasOne(m => m.User)
               .WithMany(m => m.RefreshTokens)
               .HasForeignKey(m => m.UserId);
            //.OnDelete(DeleteBehavior.Restrict);
        }
    }
}
