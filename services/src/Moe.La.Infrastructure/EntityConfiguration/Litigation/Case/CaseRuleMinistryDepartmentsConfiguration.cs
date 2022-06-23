using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    public class CaseRuleMinistryDepartmentsConfiguration : BaseEntityConfiguration<CaseRuleMinistryDepartment, int>
    {
        public override void Configure(EntityTypeBuilder<CaseRuleMinistryDepartment> builder)
        {
            builder.ToTable("CaseRuleMinistryDepartments", "Case");

            builder.HasOne(m => m.CaseRule)
                .WithMany(m => m.CaseRuleMinistryDepartments)
                .HasForeignKey(m => m.CaseRuleId)
                .IsRequired();
            //.OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.MinistryDepartment)
                .WithMany()
                .HasForeignKey(m => m.MinistryDepartmentId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            base.Configure(builder);
        }
    }
}
