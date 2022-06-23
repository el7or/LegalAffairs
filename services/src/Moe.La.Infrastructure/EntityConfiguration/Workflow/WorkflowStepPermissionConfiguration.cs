using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;
using System;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    public class WorkflowStepPermissionConfiguration : BaseEntityConfiguration<WorkflowStepPermission, Guid>
    {
        public override void Configure(EntityTypeBuilder<WorkflowStepPermission> builder)
        {
            builder.ToTable("WorkflowStepsPermissions", "BPM");

            builder.Property(m => m.RoleId)
                .IsRequired()
                .HasComment("The role id");

            builder.HasOne(m => m.WorkflowStep)
                .WithMany(m => m.WorkflowStepsPermissions)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            base.Configure(builder);
        }
    }
}
