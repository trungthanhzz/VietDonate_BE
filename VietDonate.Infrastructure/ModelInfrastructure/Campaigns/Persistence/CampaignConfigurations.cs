using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VietDonate.Domain.Model.Campaigns;

namespace VietDonate.Infrastructure.ModelInfrastructure.Campaigns.Persistence
{
    public class CampaignConfigurations : IEntityTypeConfiguration<Campaign>
    {
        public void Configure(EntityTypeBuilder<Campaign> builder)
        {
            builder.ToTable("Campaigns");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Code)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(c => c.CreatedDate)
                .IsRequired();

            builder.Property(c => c.ShortDescription)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(c => c.FullStory)
                .IsRequired(false);

            builder.Property(c => c.TargetAmount)
                .HasPrecision(18, 2)
                .IsRequired(false);

            builder.Property(c => c.CurrentAmount)
                .HasPrecision(18, 2)
                .HasDefaultValue(0m)
                .IsRequired(false);

            builder.Property(c => c.Type)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(c => c.UrgencyLevel)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(c => c.Status)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(c => c.AllowComment)
                .HasDefaultValue(true)
                .IsRequired();

            builder.Property(c => c.AllowDonate)
                .HasDefaultValue(true)
                .IsRequired();

            builder.Property(c => c.TargetItems)
                .HasColumnType("text")
                .IsUnicode(false)
                .IsRequired(false);

            builder.Property(c => c.CurrentItems)
                .HasColumnType("text")
                .IsUnicode(false)
                .IsRequired(false);

            builder.Property(c => c.StartTime)
                .IsRequired(false);

            builder.Property(c => c.EndTime)
                .IsRequired(false);

            builder.Property(c => c.ApprovedTime)
                .IsRequired(false);

            builder.Property(c => c.CompletedTime)
                .IsRequired(false);

            builder.Property(c => c.VerificationNote)
                .IsRequired(false);

            builder.Property(c => c.RejectionReason)
                .IsRequired(false);

            builder.Property(c => c.FactCheckNote)
                .IsRequired(false);

            builder.Property(c => c.ViewCount)
                .HasDefaultValue(0)
                .IsRequired();

            builder.Property(c => c.DonorCount)
                .HasDefaultValue(0)
                .IsRequired();

            builder.Property(c => c.CreateTime)
                .IsRequired(false);

            builder.Property(c => c.UpdateTime)
                .IsRequired(false);

            builder.Property(c => c.ApprovedId)
                .IsRequired(false);

            builder.Property(c => c.OwnerId)
                .IsRequired();

            builder.HasOne(c => c.ApprovedByUser)
                .WithMany()
                .HasForeignKey(c => c.ApprovedId)
                .HasConstraintName("FK_Campaigns_UserIdentities_ApprovedId")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.OwnerUser)
                .WithMany()
                .HasForeignKey(c => c.OwnerId)
                .HasConstraintName("FK_Campaigns_UserIdentities_OwnerId")
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

