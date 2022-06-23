using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities.RequestsHistory;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    public class RequestLetterHistoryConfiguration : IEntityTypeConfiguration<RequestLetterHistory>
    {
        public void Configure(EntityTypeBuilder<RequestLetterHistory> builder)
        {
            builder.ToTable("RequestLetterHistorys", "Request");

            builder.HasKey(m => m.RequestId);

            builder.Property(m => m.Text)
             .HasComment("The text of the template")
             .HasColumnType("nvarchar(max)")
             .IsRequired();
        }
    }
}
