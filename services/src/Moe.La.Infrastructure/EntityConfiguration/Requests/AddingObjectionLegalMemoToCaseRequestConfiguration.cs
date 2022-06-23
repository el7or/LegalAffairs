using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    public class AddingObjectionLegalMemoToCaseRequestConfiguration : IEntityTypeConfiguration<AddingObjectionLegalMemoToCaseRequest>
    {
        public void Configure(EntityTypeBuilder<AddingObjectionLegalMemoToCaseRequest> builder)
        {
            builder.ToTable("AddingObjectionLegalMemoToCaseRequest", "Request");

            builder.HasOne(m => m.LegalMemo)
                .WithMany()
                .HasForeignKey(m => m.LegalMemoId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.Request)
               .WithOne()
               .HasForeignKey<AddingObjectionLegalMemoToCaseRequest>(r => r.Id)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(m => m.Case)
                .WithMany()
                .HasForeignKey(m => m.CaseId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
