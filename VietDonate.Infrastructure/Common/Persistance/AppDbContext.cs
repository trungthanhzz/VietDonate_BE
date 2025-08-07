using Microsoft.EntityFrameworkCore;
using VietDonate.Domain.Campaigns;
using VietDonate.Domain.Common;
using VietDonate.Domain.Model.User;

namespace VietDonate.Infrastructure.Common.Persistance;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<UserIdentity> UserIdentities { get; set; } = null!;
    public DbSet<Campaign> Campaigns { get; set; } = null!;
    public DbSet<UserInformation> UserInformations { get; set; } = null!;
    public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}