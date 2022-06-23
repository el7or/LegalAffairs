using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    public class WorkflowStepCategoryConfiguration : BaseEntityConfiguration<WorkflowStepCategory, int>
    {
        public override void Configure(EntityTypeBuilder<WorkflowStepCategory> builder)
        {
            builder.ToTable("WorkflowStepsCategories", "BPM");

            builder.Property(m => m.Id).ValueGeneratedNever();

            builder.Property(m => m.CategoryArName)
                .HasMaxLength(50)
                .IsRequired()
                .HasComment("The workflow category Arabic name");

            base.Configure(builder);
        }
    }
}
