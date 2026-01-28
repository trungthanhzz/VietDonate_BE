using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VietDonate.Domain.Model.Transactions;

namespace VietDonate.Infrastructure.ModelInfrastructure.Transactions.Persistence
{
    public class TransactionConfigurations : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.ToTable("Transactions");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.DonationId)
                .IsRequired(false);

            builder.Property(t => t.Gateway)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(t => t.GatewayTransactionId)
                .HasMaxLength(100)
                .IsUnicode(false)
                .IsRequired(false);

            builder.Property(t => t.Amount)
                .HasPrecision(15, 2)
                .IsRequired();

            builder.Property(t => t.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired(false);

            builder.Property(t => t.FailureReason)
                .HasColumnType("text")
                .IsRequired(false);

            builder.Property(t => t.GatewayResponse)
                .HasColumnType("text")
                .IsRequired(false);

            builder.Property(t => t.CreatedTime)
                .IsRequired();

            builder.Property(t => t.CompletedTime)
                .IsRequired(false);

            builder.HasOne(t => t.Donation)
                .WithMany()
                .HasForeignKey(t => t.DonationId)
                .HasConstraintName("FK_Transactions_Donations_DonationId")
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

