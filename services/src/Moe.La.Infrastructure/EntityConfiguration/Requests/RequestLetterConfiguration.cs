using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    public class RequestLetterConfiguration : IEntityTypeConfiguration<RequestLetter>
    {
        public void Configure(EntityTypeBuilder<RequestLetter> builder)
        {
            builder.ToTable("RequestLetters", "Request");

            builder.HasKey(m => m.RequestId);

            builder.Property(m => m.Text)
             .HasComment("The text of the template")
             .HasColumnType("nvarchar(max)")
             .IsRequired();
        }
    }
}
