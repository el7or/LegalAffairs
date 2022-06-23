using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;
using System;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    public class WorkflowActionConfiguration : BaseEntityConfiguration<WorkflowAction, Guid>
    {
        public override void Configure(EntityTypeBuilder<WorkflowAction> builder)
        {
            builder.ToTable("WorkflowActions", "BPM");

            builder.Property(m => m.ActionArName)
                .HasMaxLength(50)
                .HasComment("Workflow action Arabic name")
                .IsRequired();

            builder.Property(m => m.WorkflowTypeId)
                .HasComment("The workflow type");

            builder.HasOne(m => m.WorkflowType)
                .WithMany(m => m.WorkflowActions)
                .HasForeignKey(m => m.WorkflowTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            base.Configure(builder);
        }
    }
}
