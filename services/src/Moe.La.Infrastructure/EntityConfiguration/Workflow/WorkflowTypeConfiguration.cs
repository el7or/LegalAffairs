using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;
using System;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    public class WorkflowTypeConfiguration : BaseEntityConfiguration<WorkflowType, Guid>
    {
        public override void Configure(EntityTypeBuilder<WorkflowType> builder)
        {
            builder.ToTable("WorkflowTypes", "BPM");

            builder.Property(m => m.TypeArName)
                .HasMaxLength(50)
                .IsRequired()
                .HasComment("The workflow type Arabic name");

            builder.Property(m => m.IsActive)
                .HasDefaultValue(false)
                .IsRequired()
                .HasComment("Determines wether the workflow type is active or not");

            builder.Property(m => m.LockPeriod)
                .IsRequired()
                .HasComment("Determines the amount of time allowed for this workflow type to be locked by a given user");

            base.Configure(builder);
        }
    }
}
