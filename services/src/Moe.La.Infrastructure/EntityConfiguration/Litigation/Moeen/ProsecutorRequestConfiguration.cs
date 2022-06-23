using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities.Integration.Moeen;

namespace Moe.La.Infrastructure.EntityConfiguration.Moeen
{
    public class ProsecutorRequestConfiguration : IEntityTypeConfiguration<ProsecutorRequest>
    {
        public void Configure(EntityTypeBuilder<ProsecutorRequest> builder)
        {
            builder.ToTable("ProsecutorRequests", "Moeen");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.ProsecutorRequestSubject)
               .HasComment("وصف طلب المدعي")
               .IsRequired();

            builder.Property(m => m.ProsecutorRequestOrder)
               .HasComment("ترتيب الطلب")
               .IsRequired();
        }
    }
}
