using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    public class ChangeResearcherRequestConfiguration : IEntityTypeConfiguration<ChangeResearcherRequest>
    {
        public void Configure(EntityTypeBuilder<ChangeResearcherRequest> builder)
        {
            builder.ToTable("ChangeResearcherRequests", "Request");

            builder.Property(m => m.CurrentResearcherId)
                .HasComment("The reseacher to be changed")
                .IsRequired();

            builder.Property(m => m.NewResearcherId)
                .HasComment("The researcher to be substituted with");

            builder.HasOne(m => m.CurrentResearcher)
               .WithMany()
               .HasForeignKey(m => m.CurrentResearcherId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.NewResearcher)
               .WithMany()
               .HasForeignKey(m => m.NewResearcherId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.Request)
                .WithOne()
                .HasForeignKey<ChangeResearcherRequest>(m => m.Id)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(m => m.Case)
                .WithMany()
                .HasForeignKey(m => m.CaseId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
