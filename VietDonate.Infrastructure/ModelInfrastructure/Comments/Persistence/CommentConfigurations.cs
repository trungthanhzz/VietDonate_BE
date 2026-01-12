using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VietDonate.Domain.Model.Comments;

namespace VietDonate.Infrastructure.ModelInfrastructure.Comments.Persistence
{
    public class CommentConfigurations : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable("Comments");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Content)
                .IsRequired()
                .HasColumnType("text");

            builder.Property(c => c.Type)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(c => c.UserId)
                .IsRequired();

            builder.Property(c => c.PostId)
                .IsRequired(false);

            builder.Property(c => c.ParentId)
                .IsRequired(false);

            builder.Property(c => c.IsHidden)
                .HasDefaultValue(false)
                .IsRequired();

            builder.Property(c => c.ReasonHiden)
                .HasColumnType("text")
                .IsRequired(false);

            builder.Property(c => c.HidentBy)
                .IsRequired(false);

            builder.Property(c => c.CreateTime)
                .IsRequired();

            builder.Property(c => c.UpdateTime)
                .IsRequired(false);

            builder.HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .HasConstraintName("FK_Comments_UserIdentities_UserId")
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(c => c.Post)
                .WithMany()
                .HasForeignKey(c => c.PostId)
                .HasConstraintName("FK_Comments_Posts_PostId")
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(c => c.Parent)
                .WithMany(c => c.Replies)
                .HasForeignKey(c => c.ParentId)
                .HasConstraintName("FK_Comments_Comments_ParentId")
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(c => c.HiddenByUser)
                .WithMany()
                .HasForeignKey(c => c.HidentBy)
                .HasConstraintName("FK_Comments_UserIdentities_HidentBy")
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}

