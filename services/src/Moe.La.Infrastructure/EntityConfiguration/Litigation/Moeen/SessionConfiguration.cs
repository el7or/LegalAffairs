using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities.Integration.Moeen;

namespace Moe.La.Infrastructure.EntityConfiguration.Moeen
{
    public class SessionConfiguration : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.ToTable("Sessions", "Moeen");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.SessionDate)
                .HasComment("تاريخ الجلسة")
                .IsRequired();

            builder.Property(m => m.SessionDateHijri)
                .HasComment("تاريخ الجلسة هجري")
                .IsRequired();

            builder.Property(m => m.SessionDayArabic)
                    .HasComment("يوم الجلسة")
                    .IsRequired();

            builder.Property(m => m.SessionTime)
                    .HasComment("وقت الجلسة")
                    .IsRequired();

            builder.Property(m => m.SessionTypeCode)
                    .HasComment("معرف نوع الجلسة حسب معرفات ديوان المظالم")
                    .IsRequired();

            builder.Property(m => m.SessionTypeDescription)
                    .HasComment("وصف نوع الجلسة (نظر -مرافعة -نطق بالحكم -...)")
                    .IsRequired();
        }
    }
}
