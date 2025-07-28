using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VietDonate.Infrastructure.Identity;

namespace VietDonate.Infrastructure.Common.Persistance.Configurations
{
    public class AppIdentityRoleConfiguration : IEntityTypeConfiguration<AppIdentityRole>
    {
        public void Configure(EntityTypeBuilder<AppIdentityRole> builder)
        {
            builder.ToTable("Roles");
            
            // Cấu hình Id là Guid
            builder.Property(r => r.Id)
                .HasColumnType("uuid")
                .IsRequired();
            
            // Cấu hình CreateDate
            builder.Property(r => r.CreatedDate)
                .HasColumnType("timestamp with time zone")
                .IsRequired();
            
            // Cấu hình các properties mở rộng nếu có
            // builder.Property(r => r.CustomProperty).HasMaxLength(100);
        }
    }
} 