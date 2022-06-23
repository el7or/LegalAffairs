using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;
using System;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    public class WorkflowInstanceLogConfiguration : BaseEntityConfiguration<WorkflowInstanceLog, Guid>
    {
        public override void Configure(EntityTypeBuilder<WorkflowInstanceLog> builder)
        {
            builder.ToTable("WorkflowInstancesLoges", "BPM");

            builder.Property(m => m.WorkflowInstanceId)
                .HasComment("The workflow instance id");

            builder.Property(m => m.WorkflowStepId)
                .HasComment("The workflow step id");

            builder.Property(m => m.WorkflowStatusId)
                .HasComment("The workflow status id");

            builder.Property(m => m.WorkflowActionId)
                .HasComment("The workflow action id");

            builder.Property(m => m.WorkflowInstanceNote)
                .HasMaxLength(1000)
                .HasComment("The workflow instance note");

            builder.HasOne(m => m.WorkflowInstance)
                .WithMany(m => m.WorkflowInstancesLogs)
                .HasForeignKey(m => m.WorkflowInstanceId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.WorkflowStep)
                .WithMany(m => m.WorkflowInstancesLogs)
                .HasForeignKey(m => m.WorkflowStepId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.WorkflowAction)
                .WithMany(m => m.WorkflowInstancesLogs)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.WorkflowStatus)
                .WithMany(m => m.WorkflowInstancesLogs)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.CreatedByUser)
                .WithMany()
                .HasForeignKey(m => m.CreatedBy)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.UpdatedByUser)
                .WithMany()
                .HasForeignKey(m => m.UpdatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            base.Configure(builder);
        }
    }
}
