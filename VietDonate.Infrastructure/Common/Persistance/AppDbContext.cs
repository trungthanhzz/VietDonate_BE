using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VietDonate.Domain.Campaigns;
using VietDonate.Domain.Common;
using VietDonate.Infrastructure.Identity;

namespace VietDonate.Infrastructure.Common.Persistance;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<AppIdentityUser, AppIdentityRole, Guid>(options)
{
    public DbSet<Campaign> Campaigns { get; set; } = null!;
    public new DbSet<AppIdentityUser> Users { get; set; } = null!;


    public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var domainEvents = ChangeTracker.Entries<Entity>()
           .SelectMany(entry => entry.Entity.PopDomainEvents())
           .ToList();
        //await PublishDomainEvents(domainEvents);
        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }


    //private async Task PublishDomainEvents(List<IDomainEvent> domainEvents)
    //{
    //    foreach (var domainEvent in domainEvents)
    //    {
    //        await _publisher.Publish(domainEvent);
    //    }
    //}
}