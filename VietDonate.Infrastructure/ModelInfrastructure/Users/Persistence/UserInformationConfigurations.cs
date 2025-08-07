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
    public class UserInformationConfigurations : IEntityTypeConfiguration<UserInformation>
    {
        public void Configure(EntityTypeBuilder<UserInformation> builder)
        {
            builder.ToTable("UserInformations");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.FullName)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(u => u.Phone)
                .HasMaxLength(20);

            builder.Property(u => u.Email)
                .HasMaxLength(255);

            builder.Property(u => u.Address)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(u => u.AvtUrl)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(u => u.CreateTime)
                .IsRequired();

            // 1-1 relationship with shared primary key
            builder.HasOne(u => u.UserIdentity)
                .WithOne(ui => ui.UserInformation)
                .HasForeignKey<UserInformation>(u => u.Id)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
