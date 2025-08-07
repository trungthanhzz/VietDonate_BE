using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VietDonate.Domain.Model.User;

namespace VietDonate.Infrastructure.ModelInfrastructure.Users.Persistence
{
    public class RefreshTokenConfigurations : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("RefreshTokens");

            builder.HasKey(rt => rt.Id);

            builder.Property(rt => rt.UserId)
                .IsRequired();

            builder.Property(rt => rt.Token)
                .IsRequired()
                .HasMaxLength(512);

            builder.Property(rt => rt.ExpiresAt)
                .IsRequired();

            builder.Property(rt => rt.IsRemember)
                .IsRequired();

            builder.Property(rt => rt.DeviceInfo)
                .HasMaxLength(255);

            builder.Property(rt => rt.CreatedAt)
                .IsRequired();

            builder.Property(rt => rt.RevokedAt);

            // Relationship with UserIdentity (1:N)
            builder.HasOne(rt => rt.UserIdentity)
                .WithMany(ui => ui.RefreshTokens)
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Index for faster lookups
            builder.HasIndex(rt => rt.Token).IsUnique();
            builder.HasIndex(rt => rt.UserId);
        }
    }
}

