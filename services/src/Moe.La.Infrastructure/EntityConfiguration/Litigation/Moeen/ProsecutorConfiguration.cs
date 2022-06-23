using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities.Integration.Moeen;

namespace Moe.La.Infrastructure.EntityConfiguration.Moeen
{
    public class ProsecutorConfiguration : IEntityTypeConfiguration<Prosecutor>
    {
        public void Configure(EntityTypeBuilder<Prosecutor> builder)
        {
            builder.ToTable("Prosecutors", "Moeen");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.ProsecutorCode)
                .HasComment("معرف المدعي حسب معرفات ديوان المظالم")
                .IsRequired();

            builder.Property(m => m.ProsecutorName)
                .HasComment("اسم المدعي")
                .IsRequired();

            builder.Property(m => m.ParityTypeCode)
                .HasComment("معرف نوع المدعي حسب معرفات ديوان المظالم")
                .IsRequired();

            builder.Property(m => m.ParityTypeDescription)
                .HasComment("وصف نوع المدعي (فرد-شركة -....)")
                .IsRequired();
        }
    }
}
