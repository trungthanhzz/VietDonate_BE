using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using VietDonate.Domain.Model.User;

namespace VietDonate.Infrastructure.ModelInfrastructure.Users.Persistence;

public class RoleConfigurations : IEntityTypeConfiguration<Role>
{
  public void Configure(
    EntityTypeBuilder<Role> builder
  )
  {
    builder.HasKey(r => r.Id);
    builder.Property(r => r.Name)
      .IsRequired()
      .HasMaxLength(50);
    builder.Property(r => r.Description)
      .HasMaxLength(256);
    builder.HasIndex(r => r.Name).IsUnique();
  }
}