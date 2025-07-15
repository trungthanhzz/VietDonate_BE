
using CleanArchitecture.Domain.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using VietDonate.Application.Common.Mediator;
using VietDonate.Domain.Campaigns;
using VietDonate.Domain.Common;

namespace VietDonate.Infrastructure.Common.Persistance;

public class AppDbContext(DbContextOptions options, IPublisher _publisher) : DbContext(options)
{
    public DbSet<Campaign> Campaigns { get; set; } = null!;


    public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var domainEvents = ChangeTracker.Entries<Entity>()
           .SelectMany(entry => entry.Entity.PopDomainEvents())
           .ToList();
        await PublishDomainEvents(domainEvents);
        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }


    private async Task PublishDomainEvents(List<IDomainEvent> domainEvents)
    {
        foreach (var domainEvent in domainEvents)
        {
            await _publisher.Publish(domainEvent);
        }
    }
}