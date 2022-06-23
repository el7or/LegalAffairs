using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    public class ChangeResearcherToHearingRequestConfiguration : IEntityTypeConfiguration<ChangeResearcherToHearingRequest>
    {
        public void Configure(EntityTypeBuilder<ChangeResearcherToHearingRequest> builder)
        {
            builder.ToTable("ChangeResearcherToHearingRequests", "Request");

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
                .HasForeignKey<ChangeResearcherToHearingRequest>(m => m.Id)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(m => m.Hearing)
                .WithMany()
                .HasForeignKey(m => m.HearingId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(m => m.ReplyNote)
                .HasComment("The reject reason or accept reason")
                .HasMaxLength(400);
        }
    }
}
