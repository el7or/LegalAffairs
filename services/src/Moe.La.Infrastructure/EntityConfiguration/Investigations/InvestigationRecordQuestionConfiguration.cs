using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration.Record
{
    public class InvestigationRecordQuestionConfiguration : BaseEntityConfiguration<InvestigationRecordQuestion, int>
    {
        public override void Configure(EntityTypeBuilder<InvestigationRecordQuestion> builder)
        {
            builder.ToTable("InvestigationRecordQuestions", "Investigation");

            builder.HasOne(m => m.InvestigationRecord)
                .WithMany(m => m.InvestigationRecordQuestions)
                .HasForeignKey(m => m.InvestigationRecordId);
            //.OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.AssignedTo)
                .WithMany()
                .HasForeignKey(m => m.AssignedToId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.Question)
                .WithMany()
                .HasForeignKey(m => m.QuestionId)
                .OnDelete(DeleteBehavior.Restrict);

            //builder.Property(m => m.Question)
            //.HasComment("answer question")
            //.HasMaxLength(500)
            //.IsRequired();

            builder.Property(m => m.Answer)
            .HasComment("answer text")
            .HasMaxLength(500)
            .IsRequired();

            base.Configure(builder);
        }
    }
}
