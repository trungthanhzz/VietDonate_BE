using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VietDonate.Domain.Model.Withdrawals;

namespace VietDonate.Infrastructure.ModelInfrastructure.Withdrawals.Persistence
{
    public class WithdrawalConfigurations : IEntityTypeConfiguration<Withdrawal>
    {
        public void Configure(EntityTypeBuilder<Withdrawal> builder)
        {
            builder.ToTable("Withdrawals");

            builder.HasKey(w => w.Id);

            builder.Property(w => w.CampaignId)
                .IsRequired();

            builder.Property(w => w.Amount)
                .HasPrecision(15, 2)
                .IsRequired();

            builder.Property(w => w.WithdrawalDate)
                .IsRequired()
                .HasColumnType("date");

            builder.Property(w => w.Purpose)
                .IsRequired()
                .HasColumnType("text");

            builder.Property(w => w.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired(false);

            builder.Property(w => w.RequestedBy)
                .IsRequired();

            builder.Property(w => w.ApprovedBy)
                .IsRequired(false);

            builder.Property(w => w.RejectedBy)
                .IsRequired(false);

            builder.Property(w => w.RejectionReason)
                .HasColumnType("text")
                .IsRequired(false);

            builder.Property(w => w.CreatedTime)
                .IsRequired();

            builder.Property(w => w.UpdatedTime)
                .IsRequired(false);

            builder.Property(w => w.ApprovedTime)
                .IsRequired(false);

            builder.Property(w => w.CompletedTime)
                .IsRequired(false);

            builder.HasOne(w => w.Campaign)
                .WithMany()
                .HasForeignKey(w => w.CampaignId)
                .HasConstraintName("FK_Withdrawals_Campaigns_CampaignId")
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(w => w.RequestedByUser)
                .WithMany()
                .HasForeignKey(w => w.RequestedBy)
                .HasConstraintName("FK_Withdrawals_UserIdentities_RequestedBy")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(w => w.ApprovedByUser)
                .WithMany()
                .HasForeignKey(w => w.ApprovedBy)
                .HasConstraintName("FK_Withdrawals_UserIdentities_ApprovedBy")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(w => w.RejectedByUser)
                .WithMany()
                .HasForeignKey(w => w.RejectedBy)
                .HasConstraintName("FK_Withdrawals_UserIdentities_RejectedBy")
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

