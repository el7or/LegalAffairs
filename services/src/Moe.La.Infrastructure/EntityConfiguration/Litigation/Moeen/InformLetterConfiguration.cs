using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities.Integration.Moeen;

namespace Moe.La.Infrastructure.EntityConfiguration.Moeen
{
    public class InformLetterConfiguration : BaseEntityConfiguration<InformLetter, int>
    {
        public override void Configure(EntityTypeBuilder<InformLetter> builder)
        {
            builder.ToTable("InformLetters", "Moeen");

            builder.Property(m => m.CaseNo)
                .HasComment("رقم الدعوى")
                .IsRequired();

            builder.Property(m => m.CaseDate)
                .HasComment("تاريخ قيد الدعوى")
                .IsRequired();

            builder.Property(m => m.CaseDateHijri)
                .HasComment("تاريخ قيد الدعوى هجري")
                .IsRequired();

            builder.Property(m => m.CaseSubject)
                .HasComment("موضوع الدعوى")
                .IsRequired();

            builder.Property(m => m.CaseReferences)
               .HasComment("أسانيد الدعوى")
               .IsRequired();

            builder.Property(m => m.CourtID)
                .HasComment("معرف المحكمة المقام بها الدعوى حسب معرفات ديوان المظالم")
                .IsRequired();

            builder.Property(m => m.CourtDescription)
                .HasComment("اسم المحكمة المقام بها الدعوى")
                .IsRequired();

            builder.Property(m => m.CircuitID)
               .HasComment("معرف الدائرة ناظرة الدعوى حسب معرفات ديوان المظالم");

            builder.Property(m => m.CircuitDescription)
               .HasComment("اسم الدائرة ناظرة الدعوى");

            builder.Property(m => m.InformLetterTypeCode)
              .HasComment("معرف نوع خطاب الإبلاغ حسب معرفات ديوان المظالم")
              .IsRequired();

            builder.Property(m => m.InformLetterTypeDescription)
               .HasComment("وصف نوع خطاب الإبلاغ")
               .IsRequired();

            builder.Property(m => m.InformLetterHasAttachment)
                .HasComment("هل خطاب الإبلاغ به مرفقات (نعم / لا)")
                .IsRequired();

            builder.Property(m => m.InformLetterRelevantDesc)
                .HasComment("وصف سبب إرسال الخطاب (جلسة في دعوى -جلسة في طلب -تبليغ حكم-......)")
                .IsRequired();

            builder.Property(m => m.InformLetterRelevantCode)
                .HasComment("معرف سبب إرسال الخطاب حسب معرفات ديوان المظالم")
                .IsRequired();

            builder.HasMany(m => m.ProsecutorList)
                .WithOne()
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            builder.HasMany(m => m.DefendantList)
                .WithOne()
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            builder.HasOne(m => m.NextSessionInfo)
                .WithOne()
                .HasForeignKey<Session>(m => m.Id)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(m => m.ProsecutorRequestList)
                .WithOne()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.RequestInfo)
                .WithOne()
                .HasForeignKey<Request>(m => m.Id)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.RulingInfo)
                .WithOne()
                .HasForeignKey<Ruling>(m => m.Id)
                .OnDelete(DeleteBehavior.Restrict);

            base.Configure(builder);
        }
    }
}
