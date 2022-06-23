using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    public class InvestigationQuestionConfiguration : IEntityTypeConfiguration<InvestigationQuestion>
    {
        public void Configure(EntityTypeBuilder<InvestigationQuestion> builder)
        {
            builder.ToTable("InvestigationQuestions", "Lookups");

            builder.Property(m => m.Id)
                .ValueGeneratedNever();

            builder.Property(m => m.Question)
                .HasComment("The Question of the Investigation Question")
                .HasMaxLength(500)
                .IsRequired();
        }
    }
}
