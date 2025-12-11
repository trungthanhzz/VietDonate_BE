using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VietDonate.Domain.Model.Donations;

namespace VietDonate.Infrastructure.ModelInfrastructure.Donations.Persistence
{
    public class DonationConfigurations : IEntityTypeConfiguration<Donation>
    {
        public void Configure(EntityTypeBuilder<Donation> builder)
        {
            builder.ToTable("Donations");

            builder.HasKey(d => d.Id);

            builder.Property(d => d.CampaignId)
                .IsRequired();

            builder.Property(d => d.DonorId)
                .IsRequired(false);

            builder.Property(d => d.Type)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(d => d.Amount)
                .HasPrecision(15, 2)
                .IsRequired();

            builder.Property(d => d.GoodsDetails)
                .HasColumnType("text")
                .IsUnicode(false)
                .IsRequired(false);

            builder.Property(d => d.Message)
                .HasColumnType("text")
                .IsRequired(false);

            builder.Property(d => d.PaymentMethod)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired(false);

            builder.Property(d => d.PaymentStatus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired(false);

            builder.Property(d => d.TransactionId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired(false);

            builder.Property(d => d.CreateTime)
                .IsRequired();

            builder.Property(d => d.UpdateTime)
                .IsRequired(false);

            builder.HasOne(d => d.Campaign)
                .WithMany()
                .HasForeignKey(d => d.CampaignId)
                .HasConstraintName("FK_Donations_Campaigns_CampaignId")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.Donor)
                .WithMany()
                .HasForeignKey(d => d.DonorId)
                .HasConstraintName("FK_Donations_UserIdentities_DonorId")
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

