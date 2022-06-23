using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    class CaseTransactionConfiguration : BaseEntityConfiguration<CaseTransaction, int>
    {
        public override void Configure(EntityTypeBuilder<CaseTransaction> builder)
        {
            builder.ToTable("CaseTransactions", "Case");


            builder.Property(m => m.Note)
                .HasComment("The Text the CaseTransaction")
                .HasMaxLength(200);

            builder.HasOne(m => m.Case)
               .WithMany(c => c.CaseTransactions)
               .HasForeignKey(m => m.CaseId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.CreatedByUser)
                .WithMany()
                .HasForeignKey(m => m.CreatedBy)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            base.Configure(builder);
        }
    }
}
