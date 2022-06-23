using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities.Integration.Moeen;

namespace Moe.La.Infrastructure.EntityConfiguration.Moeen
{
    public class RulingConfiguration : IEntityTypeConfiguration<Ruling>
    {
        public void Configure(EntityTypeBuilder<Ruling> builder)
        {
            builder.ToTable("Rulings", "Moeen");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.CircuitID)
                .HasComment("معرف الدائرة مصدرة الحكم حسب معرفات ديوان المظالم")
                .IsRequired();

            builder.Property(m => m.CircuitID)
                .HasComment("")
                .IsRequired();

            builder.Property(m => m.CircuitDescription)
                .HasComment("اسم الدائرة مصدرة الحكم")
                .IsRequired();

            builder.Property(m => m.RulingDate)
                .HasComment("تاريخ الحكم")
                .IsRequired();

            builder.Property(m => m.RulingDateHijri)
                .HasComment("تاريخ الحكم هجري")
                .IsRequired();

            builder.Property(m => m.RulingSpoken)
                .HasComment("منطوق الحكم")
                .IsRequired();

            builder.Property(m => m.RulingDeliveryDate)
                .HasComment("التاريخ المحدد لاستلام الحكم")
                .IsRequired();

            builder.Property(m => m.RulingDeliveryDateHijri)
                .HasComment("التاريخ المحدد لاستلام الحكم هجري")
                .IsRequired();
        }
    }
}
