using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    class MoamalaTransactionConfiguration : BaseEntityConfiguration<MoamalaTransaction, int>
    {
        public override void Configure(EntityTypeBuilder<MoamalaTransaction> builder)
        {
            builder.ToTable("MoamalaTransactions", "Moamalat");


            builder.Property(m => m.Note)
                .HasComment("The Text the MoamalaTransaction")
                .HasMaxLength(100);

            builder.HasOne(m => m.Moamala)
               .WithMany(c => c.MoamalaTransactions)
               .HasForeignKey(m => m.MoamalaId)
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
