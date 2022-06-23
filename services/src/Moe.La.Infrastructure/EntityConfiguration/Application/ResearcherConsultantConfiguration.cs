using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    public class ResearcherConsultantConfiguration : BaseEntityConfiguration<ResearcherConsultant, int>
    {
        public override void Configure(EntityTypeBuilder<ResearcherConsultant> builder)
        {
            builder.ToTable("ResearcherConsultant", "Applicatoin");

            builder.Property(m => m.ConsultantId)
                .IsRequired()
                .HasComment("The consultant id");

            builder.Property(m => m.ResearcherId)
                .IsRequired()
                .HasComment("The reasearcher id");

            builder.Property(m => m.IsActive)
                .HasComment("check if researcher now connected with consultant");

            //builder.HasOne(m => m.Consultant)
            //    .WithMany(m => m.Researchers)
            //    .HasForeignKey(m => m.ConsultantId)
            // .IsRequired()
            //    .OnDelete(DeleteBehavior.Restrict);

            //builder.HasOne(m => m.Researcher)
            //    .WithMany(m => m.Consultants)
            //    .HasForeignKey(m => m.ResearcherId)
            //  .IsRequired()
            //    .OnDelete(DeleteBehavior.Restrict);

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
