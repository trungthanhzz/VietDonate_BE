using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VietDonate.Domain.Model.Categories;

namespace VietDonate.Infrastructure.ModelInfrastructure.Categories.Persistence
{
    public class CategoryConfigurations : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(c => c.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired(false);

            builder.Property(c => c.Type)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(c => c.Description)
                .HasColumnType("text")
                .IsRequired(false);

            builder.Property(c => c.CreatedBy)
                .IsRequired(false);

            builder.Property(c => c.OtherInfo)
                .HasColumnType("text")
                .IsRequired(false);

            builder.Property(c => c.CreatedTime)
                .IsRequired();

            builder.Property(c => c.UpdateTime)
                .IsRequired(false);

            builder.HasOne(c => c.CreatedByUser)
                .WithMany()
                .HasForeignKey(c => c.CreatedBy)
                .HasConstraintName("FK_Categories_UserIdentities_CreatedBy")
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

