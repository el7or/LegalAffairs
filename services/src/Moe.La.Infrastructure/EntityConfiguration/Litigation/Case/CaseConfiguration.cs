using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    class CaseConfiguration : BaseEntityConfiguration<Case, int>
    {
        public override void Configure(EntityTypeBuilder<Case> builder)
        {
            builder.ToTable("Cases", "Case");

            builder.Property(m => m.CaseSource)
                .HasComment("The source of the case (Najiz, Moeen, etc.)")
                .IsRequired();

            builder.Property(m => m.CaseNumberInSource)
                .HasComment("Case number in source")
                .HasMaxLength(30);

            builder.Property(m => m.CaseYearInSource)
                .HasComment("Case year in source")
                .IsRequired();

            builder.Property(m => m.LitigationType)
                .HasComment("The case litigation type")
                .IsRequired();

            builder.Property(m => m.ReferenceCaseNo)
                .HasComment("Reference case number")
                .HasMaxLength(30);

            builder.Property(m => m.UnifiedNumber)
                .HasComment("The case main number")
                .HasMaxLength(50);

            builder.Property(m => m.StartDate)
                .HasComment("Case start date in source")
                .IsRequired();

            builder.Property(m => m.CourtId)
                .HasComment("The court that is looking the case");

            builder.Property(m => m.CircleNumber)
                .HasComment("The circle no of the case")
                .HasMaxLength(50);

            builder.Property(m => m.Subject)
                .HasComment("The subject of the case")
                .HasMaxLength(100);

            builder.Property(m => m.CaseDescription)
                .HasComment("The case description")
                .HasMaxLength(2000);

            builder.Property(m => m.LegalStatus)
                .HasComment("The case legal status")
                .IsRequired();

            builder.Property(m => m.RelatedCaseId)
                .HasComment("The related case");

            builder.Property(m => m.Status)
                .HasComment("The case status during its lifetime")
                .IsRequired();

            builder.Property(m => m.BranchId)
                .HasComment("The related branch whom is the case owner")
                .IsRequired();

            builder.Property(m => m.IsArchived)
                .HasComment("After the case is closed, it will be archived")
                .IsRequired();

            builder.Property(m => m.FileNo)
                .HasComment("The case file number. Used for cases manual organization")
                .HasMaxLength(20);

            builder.Property(m => m.JudgeName)
                .HasComment("The judge name of the case")
                .HasMaxLength(100);

            builder.Property(m => m.CloseDate)
                .HasComment("The case close date");

            builder.Property(m => m.PronouncingJudgmentDate)
                .HasComment("Date of pronouncement of judgment");

            builder.Property(m => m.ReceivingJudgmentDate)
                .HasComment("The date of receiving the judgment");

            builder.Property(m => m.Notes)
                .HasComment("The case notes")
                .HasMaxLength(1000);

            builder.Property(m => m.SecondSubCategoryId)
                .HasComment("Case classification")
                .IsRequired();

            builder.Property(m => m.CaseClosingReason)
                .HasComment("Case closing reason");

            builder.Property(m => m.IsManual)
                .HasComment("Determines the entry method: manual or integration");

            builder.HasOne(m => m.RelatedCase)
                 .WithMany()
                 .HasForeignKey(m => m.RelatedCaseId)
                 .IsRequired(false)
                 .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.CaseRule)
                .WithOne()
                .HasForeignKey<CaseRule>(m => m.Id)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.CreatedByUser)
                .WithMany()
                .HasForeignKey(m => m.CreatedBy)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.UpdatedByUser)
                .WithMany()
                .HasForeignKey(m => m.UpdatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasQueryFilter(m => !m.IsArchived);

            builder.HasIndex(e => new { e.CaseSource, e.CaseNumberInSource, e.CaseYearInSource })
                .IsUnique()
                .HasFilter("IsDeleted = 0");

            builder.HasOne(m => m.SecondSubCategory)
                .WithMany()
                .HasForeignKey(m => m.SecondSubCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(m => m.CaseHistory)
                .WithOne()
                .HasForeignKey(m => m.CaseId)
                .OnDelete(DeleteBehavior.Restrict);

            #region Properties likely will be deleted in the future

            //builder.Property(m => m.OrderDescription)
            //   .HasComment("The case order description")
            //   .HasMaxLength(400);

            //builder.HasOne(m => m.WorkflowInstance)
            //    .WithMany()
            //    .HasForeignKey(m => m.WorkflowInstanceId)
            //    .OnDelete(DeleteBehavior.Restrict);

            //builder.Property(m => m.RaselRef)
            //    .HasComment("Rasel reference number")
            //    .HasMaxLength(30);

            //builder.Property(m => m.RaselUnifiedNo)
            //    .HasComment("Rasel unified number")
            //    .HasMaxLength(30);

            //builder.Property(m => m.NajizId)
            //    .HasComment("Najiz case ID")
            //    .HasMaxLength(30);

            //builder.Property(m => m.NajizRef)
            //    .HasComment("Najiz case reference number")
            //    .HasMaxLength(50);

            //builder.Property(m => m.MoeenRef)
            //    .HasComment("Moeen reference number")
            //    .HasMaxLength(30);

            //builder.Property(m => m.WorkflowInstanceId)
            //    .HasComment("The related workflow instance ID");

            //builder.Property(m => m.RecordDate)
            //    .HasComment("The case creation date, this value determined by the external services like Moeen, Najiz, etc.");

            #endregion

            base.Configure(builder);
        }
    }
}
