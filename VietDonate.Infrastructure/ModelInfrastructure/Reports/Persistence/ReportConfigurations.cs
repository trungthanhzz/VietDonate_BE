using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VietDonate.Domain.Model.Reports;

namespace VietDonate.Infrastructure.ModelInfrastructure.Reports.Persistence
{
    public class ReportConfigurations : IEntityTypeConfiguration<Report>
    {
        public void Configure(EntityTypeBuilder<Report> builder)
        {
            builder.ToTable("Reports");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Type)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(r => r.Description)
                .IsRequired()
                .HasColumnType("text");

            builder.Property(r => r.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired(false);

            builder.Property(r => r.ReposterId)
                .IsRequired();

            builder.Property(r => r.CampaignId)
                .IsRequired(false);

            builder.Property(r => r.ResolvedBy)
                .IsRequired(false);

            builder.Property(r => r.Note)
                .HasColumnType("text")
                .IsRequired(false);

            builder.Property(r => r.CreatedTime)
                .IsRequired();

            builder.Property(r => r.UpdatedTime)
                .IsRequired(false);

            builder.HasOne(r => r.Reposter)
                .WithMany()
                .HasForeignKey(r => r.ReposterId)
                .HasConstraintName("FK_Reports_UserIdentities_ReposterId")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.Campaign)
                .WithMany()
                .HasForeignKey(r => r.CampaignId)
                .HasConstraintName("FK_Reports_Campaigns_CampaignId")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.ResolvedByUser)
                .WithMany()
                .HasForeignKey(r => r.ResolvedBy)
                .HasConstraintName("FK_Reports_UserIdentities_ResolvedBy")
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

