using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities.Integration.Moeen;

namespace Moe.La.Infrastructure.EntityConfiguration.Moeen
{
    public class DefendantConfiguration : IEntityTypeConfiguration<Defendant>
    {
        public void Configure(EntityTypeBuilder<Defendant> builder)
        {
            builder.ToTable("Defendants", "Moeen");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.DefendantCode)
                .HasComment("معرف المدعي عليه حسب معرفات ديوان المظالم")
                .IsRequired();

            builder.Property(m => m.DefendantName)
                .HasComment("اسم المدعى عليه")
                .IsRequired();

            builder.Property(m => m.ParityTypeCode)
                .HasComment("معرف نوع المدعى عليه حسب معرفات ديوان المظالم")
                .IsRequired();

            builder.Property(m => m.ParityTypeDescription)
                .HasComment("وصف نوع المدعى عليه (فرد-شركة -....)")
                .IsRequired();
        }
    }
}
