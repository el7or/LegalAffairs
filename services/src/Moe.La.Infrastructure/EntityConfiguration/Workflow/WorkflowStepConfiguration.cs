using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;
using System;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    public class WorkflowStepConfiguration : BaseEntityConfiguration<WorkflowStep, Guid>
    {
        public override void Configure(EntityTypeBuilder<WorkflowStep> builder)
        {
            builder.ToTable("WorkflowSteps", "BPM");

            builder.Property(m => m.StepArName)
                .HasMaxLength(50)
                .IsRequired()
                .HasComment("The workflow step Arabic name");

            builder.Property(m => m.WorkflowTypeId)
                .HasComment("The workflow type id");

            builder.Property(m => m.WorkflowStepCategoryId)
                .HasComment("The work flow step category id");

            builder.HasOne(m => m.WorkflowType)
                .WithMany(m => m.WorkflowSteps)
                .HasForeignKey(m => m.WorkflowTypeId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.WorkflowStepCategory)
                .WithMany(m => m.WorkflowSteps)
                .HasForeignKey(m => m.WorkflowStepCategoryId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            base.Configure(builder);
        }
    }
}
