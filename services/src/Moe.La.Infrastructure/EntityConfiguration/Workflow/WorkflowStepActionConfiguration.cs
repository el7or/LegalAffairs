using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;
using System;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    public class WorkflowStepActionConfiguration : BaseEntityConfiguration<WorkflowStepAction, Guid>
    {
        public override void Configure(EntityTypeBuilder<WorkflowStepAction> builder)
        {
            builder.ToTable("WorkflowStepsActions", "BPM");

            builder.Property(m => m.WorkflowStepId)
                .HasComment("The workflow step id");

            builder.Property(m => m.WorkflowActionId)
                .HasComment("The workflow action id");

            builder.Property(m => m.NextStatusId)
                .HasComment("The next status id");

            builder.Property(m => m.NextStepId)
                .HasComment("The next step for the workflow instance");

            builder.Property(m => m.Description)
                .HasComment("The step action description");

            builder.Property(m => m.IsNoteVisible)
                .HasComment("Determines wether the note field is visible or not");

            builder.Property(m => m.IsNoteRequired)
                .HasComment("Determines wether the note field is required or not");

            builder.Property(m => m.Description)
                .HasMaxLength(100)
                .IsRequired();

            builder.HasOne(m => m.WorkflowStep)
                .WithMany(m => m.WorkflowCurrentSteps)
                .HasForeignKey(m => m.WorkflowStepId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.WorkflowAction)
                .WithMany(m => m.WorkflowStepsActions)
                .HasForeignKey(m => m.WorkflowActionId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.NextStep)
                .WithMany(m => m.WorkflowNextSteps)
                .HasForeignKey(m => m.NextStepId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.NextStatus)
                .WithMany(m => m.WorkflowNextStatuses)
                .HasForeignKey(m => m.NextStatusId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            base.Configure(builder);
        }
    }
}
