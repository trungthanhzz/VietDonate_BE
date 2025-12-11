using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VietDonate.Domain.Model.Media;

namespace VietDonate.Infrastructure.ModelInfrastructure.Media.Persistence
{
    public class MediaConfigurations : IEntityTypeConfiguration<VietDonate.Domain.Model.Media.Media>
    {
        public void Configure(EntityTypeBuilder<VietDonate.Domain.Model.Media.Media> builder)
        {
            builder.ToTable("Media");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.Type)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(m => m.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired(false);

            builder.Property(m => m.PostId)
                .IsRequired(false);

            builder.Property(m => m.UserId)
                .IsRequired(false);

            builder.Property(m => m.CampaignId)
                .IsRequired(false);

            builder.Property(m => m.Path)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(m => m.DisplayOrder)
                .HasDefaultValue(0)
                .IsRequired();

            builder.Property(m => m.CreateTime)
                .IsRequired();

            builder.Property(m => m.UpdateTime)
                .IsRequired(false);

            builder.HasOne(m => m.Post)
                .WithMany()
                .HasForeignKey(m => m.PostId)
                .HasConstraintName("FK_Media_Posts_PostId")
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(m => m.User)
                .WithMany()
                .HasForeignKey(m => m.UserId)
                .HasConstraintName("FK_Media_UserIdentities_UserId")
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(m => m.Campaign)
                .WithMany()
                .HasForeignKey(m => m.CampaignId)
                .HasConstraintName("FK_Media_Campaigns_CampaignId")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

