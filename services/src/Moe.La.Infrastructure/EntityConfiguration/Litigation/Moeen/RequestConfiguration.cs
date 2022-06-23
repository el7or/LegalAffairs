using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities.Integration.Moeen;

namespace Moe.La.Infrastructure.EntityConfiguration.Moeen
{
    public class RequestConfiguration : IEntityTypeConfiguration<Request>
    {
        public void Configure(EntityTypeBuilder<Request> builder)
        {
            builder.ToTable("Requests", "Moeen");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.RequestSubject)
                .HasComment("موضوع الطلب")
                .IsRequired();

            builder.Property(m => m.RequestDate)
                .HasComment("تاريخ تقديم الطلب")
                .IsRequired();

            builder.Property(m => m.RequestDateHijri)
                .HasComment("تاريخ تقديم الطلب هجري")
                .IsRequired();

            builder.Property(m => m.RequestTypeCode)
                .HasComment("معرف نوع الطلب حسب معرفات ديوان المظالم")
                .IsRequired();

            builder.Property(m => m.RequestTypeDescription)
                .HasComment("وصف نوع الطلب")
                .IsRequired();

            builder.Property(m => m.RequesterName)
                .HasComment("اسم مقدم الطلب");

            builder.Property(m => m.RequesterParityTypeCode)
                .HasComment("معرف صفة مقدم الطلب حسب معرفات ديوان المظالم");

            builder.Property(m => m.RequesterParityTypeDescription)
                .HasComment("وصف صفة مقدم الطلب");
        }
    }
}
