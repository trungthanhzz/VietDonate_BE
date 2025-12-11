using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VietDonate.Domain.Model.Likes;

namespace VietDonate.Infrastructure.ModelInfrastructure.Likes.Persistence
{
    public class LikeConfigurations : IEntityTypeConfiguration<Like>
    {
        public void Configure(EntityTypeBuilder<Like> builder)
        {
            builder.ToTable("Likes");

            builder.HasKey(l => l.Id);

            builder.Property(l => l.Type)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(l => l.UserId)
                .IsRequired();

            builder.Property(l => l.PostId)
                .IsRequired(false);

            builder.Property(l => l.CreatedTime)
                .IsRequired();

            builder.HasOne(l => l.User)
                .WithMany()
                .HasForeignKey(l => l.UserId)
                .HasConstraintName("FK_Likes_UserIdentities_UserId")
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(l => l.Post)
                .WithMany()
                .HasForeignKey(l => l.PostId)
                .HasConstraintName("FK_Likes_Posts_PostId")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

