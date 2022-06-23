using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    public class CaseRuleProsecutorRequestConfiguration : BaseEntityConfiguration<CaseRuleProsecutorRequest, int>
    {
        public override void Configure(EntityTypeBuilder<CaseRuleProsecutorRequest> builder)

        {
            builder.ToTable("CaseRuleProsecutorRequest", "Case");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.ProsecutorRequestSubject)
               .HasComment("وصف طلب المدعي")
               .IsRequired();

            builder.Property(m => m.ProsecutorRequestOrder)
               .HasComment("ترتيب الطلب")
               .IsRequired();

            builder.Property(m => m.CaseId)
                .HasComment("The case id");

            builder.HasOne(m => m.Case)
                .WithMany(m => m.ProsecutorRequests)
                .HasForeignKey(m => m.CaseId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
