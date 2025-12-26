using Microsoft.EntityFrameworkCore;
using VietDonate.Domain.Common;
using VietDonate.Domain.Model.CampaignCategories;
using VietDonate.Domain.Model.Campaigns;
using VietDonate.Domain.Model.Categories;
using VietDonate.Domain.Model.Comments;
using VietDonate.Domain.Model.Donations;
using VietDonate.Domain.Model.Likes;
using VietDonate.Domain.Model.Media;
using VietDonate.Domain.Model.Notifications;
using VietDonate.Domain.Model.Posts;
using VietDonate.Domain.Model.Reports;
using VietDonate.Domain.Model.Transactions;
using VietDonate.Domain.Model.User;
using VietDonate.Domain.Model.Withdrawals;

namespace VietDonate.Infrastructure.Common.Persistance;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<UserIdentity> UserIdentities { get; set; } = null!;
    public DbSet<Campaign> Campaigns { get; set; } = null!;
    public DbSet<UserInformation> UserInformations { get; set; } = null!;
    public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;
    public DbSet<Donation> Donations { get; set; } = null!;
    public DbSet<Post> Posts { get; set; } = null!;
    public DbSet<Comment> Comments { get; set; } = null!;
    public DbSet<Like> Likes { get; set; } = null!;
    public DbSet<Report> Reports { get; set; } = null!;
    public DbSet<Media> Media { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<CampaignCategory> CampaignCategories { get; set; } = null!;
    public DbSet<Notification> Notifications { get; set; } = null!;
    public DbSet<Transaction> Transactions { get; set; } = null!;
    public DbSet<Withdrawal> Withdrawals { get; set; } = null!;

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