using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;
using System;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    public class WorkflowInstanceConfiguration : BaseEntityConfiguration<WorkflowInstance, Guid>
    {
        public override void Configure(EntityTypeBuilder<WorkflowInstance> builder)
        {
            builder.ToTable("WorkflowInstances", "BPM");

            builder.Property(m => m.WorkflowTypeId)
                .HasComment("The workflow type id");

            builder.Property(m => m.CurrentStepId)
                .HasComment("The current step id where the this workflow instance is in");

            builder.Property(m => m.CurrentStatusId)
                .HasComment("The current status id");

            builder.Property(m => m.WorkflowStepPermissionId)
                .HasComment("The current step permission id for this workflow instance");

            builder.Property(m => m.LockedOn)
                .HasComment("The locked on datetime")
                .IsRequired(false);

            builder.Property(m => m.LockedBy)
                .HasComment("The user's id whom locked by")
                .IsRequired(false);

            builder.Property(m => m.ClaimedOn)
                .HasComment("The claimed on datetime")
                .IsRequired(false);

            builder.Property(m => m.ClaimedBy)
                .HasComment("The user's id whom claimed by")
                .IsRequired(false);

            builder.Property(m => m.AssignedTo)
                .HasComment("The user's id whom assigned to")
                .IsRequired(false);

            builder.HasOne(m => m.WorkflowType)
                .WithMany(m => m.WorkflowInstances)
                .HasForeignKey(m => m.WorkflowTypeId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.CurrentStep)
                .WithMany(m => m.WorkflowInstances)
                .HasForeignKey(m => m.CurrentStepId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.CurrentStatus)
                .WithMany(m => m.WorkflowInstances)
                .HasForeignKey(m => m.CurrentStatusId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.WorkflowStepPermission)
                .WithMany(m => m.WorkflowInstances)
                .HasForeignKey(m => m.WorkflowStepPermissionId)
                .IsRequired(false)
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
