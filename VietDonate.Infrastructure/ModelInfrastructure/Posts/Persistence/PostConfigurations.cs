using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VietDonate.Domain.Model.Posts;

namespace VietDonate.Infrastructure.ModelInfrastructure.Posts.Persistence
{
    public class PostConfigurations : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.ToTable("Posts");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Title)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(p => p.Content)
                .IsRequired()
                .HasColumnType("text");

            builder.Property(p => p.PostType)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(p => p.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired(false);

            builder.Property(p => p.UserId)
                .IsRequired();

            builder.Property(p => p.CampaignId)
                .IsRequired(false);

            builder.Property(p => p.ViewCount)
                .HasDefaultValue(0)
                .IsRequired();

            builder.Property(p => p.LikeCount)
                .HasDefaultValue(0)
                .IsRequired();

            builder.Property(p => p.CommentCount)
                .HasDefaultValue(0)
                .IsRequired();

            builder.Property(p => p.ProofType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired(false);

            builder.Property(p => p.ProofDate)
                .IsRequired(false);

            builder.Property(p => p.CreateTime)
                .IsRequired();

            builder.Property(p => p.UpdateTime)
                .IsRequired(false);

            builder.HasOne(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.UserId)
                .HasConstraintName("FK_Posts_UserIdentities_UserId")
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.Campaign)
                .WithMany()
                .HasForeignKey(p => p.CampaignId)
                .HasConstraintName("FK_Posts_Campaigns_CampaignId")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

