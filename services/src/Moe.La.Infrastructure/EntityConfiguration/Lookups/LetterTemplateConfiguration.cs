using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    class LetterTemplateConfiguration : IEntityTypeConfiguration<LetterTemplate>
    {
        public void Configure(EntityTypeBuilder<LetterTemplate> builder)
        {
            builder.ToTable("LetterTemplate", "Lookups");

            builder.Property(m => m.Id)
                .ValueGeneratedNever();

            builder.Property(m => m.Name)
                .HasComment("The name of the template")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(m => m.Type)
               .HasComment("The type of the template")
               .IsRequired();

            builder.Property(m => m.Text)
              .HasComment("The content of the template")
              .HasColumnType("nvarchar(max)")
              .IsRequired();

            builder.Property(m => m.Thumbnail)
               .HasComment("The name of the template")
               .HasMaxLength(500)
               .IsRequired();
        }
    }
}
