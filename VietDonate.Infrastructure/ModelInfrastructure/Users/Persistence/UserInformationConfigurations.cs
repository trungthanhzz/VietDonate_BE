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
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.Property(u => u.Email)
                .HasMaxLength(255)
                .IsUnicode(false);

            builder.Property(u => u.Address)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(u => u.AvtUrl)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(u => u.CreateTime)
                .IsRequired();

            builder.Property(u => u.DateOfBirth)
                .IsRequired(false);

            builder.Property(u => u.Status)
                .HasMaxLength(50)
                .IsRequired(false);

            builder.Property(u => u.VerificationStatus)
                .HasMaxLength(50)
                .IsRequired(false);

            builder.Property(u => u.IdentityNumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired(false);

            builder.Property(u => u.OrganizationName)
                .HasMaxLength(255)
                .IsRequired(false);

            builder.Property(u => u.OrganizationTaxCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired(false);

            builder.Property(u => u.OrganizationRegisterNumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired(false);

            builder.Property(u => u.OrganizationLegalRepresentative)
                .HasMaxLength(255)
                .IsRequired(false);

            builder.Property(u => u.BankAccountNumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired(false);

            builder.Property(u => u.BankName)
                .HasMaxLength(255);

            builder.Property(u => u.BankBranch)
                .HasMaxLength(255);

            builder.Property(u => u.TotalDonated)
                .HasPrecision(15, 2)
                .HasDefaultValue(0m)
                .IsRequired();

            builder.Property(u => u.TotalRecieved)
                .HasPrecision(15, 2)
                .HasDefaultValue(0m)
                .IsRequired();

            builder.Property(u => u.CampaignCount)
                .HasDefaultValue(0)
                .IsRequired();

            builder.Property(u => u.StaffNumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired(false);

            builder.Property(u => u.UpdateTime)
                .IsRequired(false);

            builder.HasOne(u => u.UserIdentity)
                .WithOne(ui => ui.UserInformation)
                .HasForeignKey<UserInformation>(u => u.Id)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
