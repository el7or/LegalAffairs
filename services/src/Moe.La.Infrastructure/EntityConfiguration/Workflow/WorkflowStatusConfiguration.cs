using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    public class WorkflowStatusConfiguration : BaseEntityConfiguration<WorkflowStatus, int> //BaseEntityConfiguration<WorkflowStatus, int>
    {
        public override void Configure(EntityTypeBuilder<WorkflowStatus> builder)
        {
            builder.ToTable("WorkflowStatuses", "BPM");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.Id).ValueGeneratedNever();

            builder.Property(m => m.StatusArName)
                .HasMaxLength(50)
                .HasComment("The workflow status Arabic name")
                .IsRequired();

            base.Configure(builder);
        }
    }
}
