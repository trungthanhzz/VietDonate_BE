using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VietDonate.Domain.Model.Notifications;

namespace VietDonate.Infrastructure.ModelInfrastructure.Notifications.Persistence
{
    public class NotificationConfigurations : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable("Notifications");

            builder.HasKey(n => n.Id);

            builder.Property(n => n.UserId)
                .IsRequired();

            builder.Property(n => n.Title)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(n => n.Content)
                .IsRequired()
                .HasColumnType("text");

            builder.Property(n => n.Type)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired(false);

            builder.Property(n => n.IsRead)
                .HasDefaultValue(false)
                .IsRequired();

            builder.Property(n => n.CreatedTime)
                .IsRequired();

            builder.Property(n => n.UpdatedTime)
                .IsRequired(false);

            builder.HasOne(n => n.User)
                .WithMany()
                .HasForeignKey(n => n.UserId)
                .HasConstraintName("FK_Notifications_UserIdentities_UserId")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

