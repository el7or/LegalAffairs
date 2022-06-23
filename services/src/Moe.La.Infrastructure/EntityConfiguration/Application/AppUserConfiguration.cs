using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moe.La.Core.Entities;

namespace Moe.La.Infrastructure.EntityConfiguration
{
    class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.ToTable("Users", "Application");

            builder.Property(m => m.FirstName)
                .HasComment("The user's first name.")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(m => m.SecondName)
                .HasComment("The user's second name.")
                .HasMaxLength(50);

            builder.Property(m => m.ThirdName)
                .HasComment("The user's third name.")
                .HasMaxLength(50);

            builder.Property(m => m.LastName)
                .HasComment("The user's last name.")
                .HasMaxLength(50);

            builder.Property(m => m.BranchId)
                .HasComment("The user's General management or branch which belong to.");

            builder.Property(m => m.IdentityNumber)
                .HasComment("The identity number")
                .HasMaxLength(10);

            builder.Property(m => m.EmployeeNo)
                .HasComment("The employee number")
                .HasMaxLength(10)
                .IsRequired(false);

            builder.Property(m => m.ExtensionNumber)
                .HasComment("The extension number")
                .HasMaxLength(4)
                .IsRequired(false);

            builder.Property(m => m.Picture)
                .HasComment("The user's picture uniqueidentifier + extension. Used to retrieve the user's picture from the file store.")
                .HasMaxLength(50);

            builder.Property(m => m.Signature)
                .HasColumnType("varbinary(max)")
                .HasComment("The user's signature binary image.");

            builder.Property(m => m.CreatedOn)
               .HasComment("The creation datetime.")
               .IsRequired();

            builder.HasOne(m => m.Branch)
                .WithMany()
                .HasForeignKey(m => m.BranchId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.JobTitle)
                .WithMany()
                .HasForeignKey(m => m.JobTitleId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(m => m.Consultants)
                .WithOne(m => m.Researcher)
                .HasForeignKey(m => m.ResearcherId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(m => m.Researchers)
                .WithOne(m => m.Consultant)
                .HasForeignKey(m => m.ConsultantId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasQueryFilter(m => !m.IsDeleted);
        }
    }
}
