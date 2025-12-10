using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VietDonate.Domain.Model.User;

namespace VietDonate.Infrastructure.ModelInfrastructure.Users.Persistence
{
    public class UserIndentityConfigurations : IEntityTypeConfiguration<UserIdentity>
    {
        public void Configure(EntityTypeBuilder<UserIdentity> builder)
        {
            builder.ToTable("UserIdentities");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.UserName)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(u => u.NormalizedUserName)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(u => u.PasswordHash)
                .IsRequired();

            builder.Property(u => u.IsActive)
                .IsRequired();

            builder.Property(u => u.ConcurrenceStamp)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(u => u.SecurityStamp)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(u => u.CreatedDate)
                .IsRequired();

            builder.Property(u => u.RoleType)
                .HasConversion<string>()
                .HasMaxLength(10);

            builder.HasIndex(u => u.NormalizedUserName).IsUnique();

            builder.HasOne(u => u.UserInformation)
                .WithOne(ui => ui.UserIdentity)
                .HasForeignKey<UserInformation>(ui => ui.Id)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
