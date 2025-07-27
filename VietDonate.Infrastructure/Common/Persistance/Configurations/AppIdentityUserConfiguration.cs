using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VietDonate.Infrastructure.Identity;

namespace VietDonate.Infrastructure.Common.Persistance.Configurations
{
    public class AppIdentityUserConfiguration : IEntityTypeConfiguration<AppIdentityUser>
    {
        public void Configure(EntityTypeBuilder<AppIdentityUser> builder)
        {
            builder.ToTable("Users");
            
            // Cấu hình Id là Guid
            builder.Property(u => u.Id)
                .HasColumnType("uuid")
                .IsRequired();
            
            // Cấu hình CreateDate
            builder.Property(u => u.CreatedDate)
                .HasColumnType("timestamp with time zonee")
                .IsRequired();
            
            // Cấu hình các properties mở rộng nếu có
            // builder.Property(u => u.CustomProperty).HasMaxLength(100);
        }
    }
} 