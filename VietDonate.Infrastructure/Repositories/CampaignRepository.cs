using Microsoft.EntityFrameworkCore;
using VietDonate.Application.Common.Interfaces.IRepository;
using VietDonate.Domain.Model.Campaigns;
using VietDonate.Infrastructure.Common.Persistance;

namespace VietDonate.Infrastructure.Repositories
{
    public class CampaignRepository : ICampaignRepository
    {
        readonly AppDbContext _context;
        public CampaignRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Campaign user, CancellationToken cancellationToken)
        {
            await _context.AddAsync(user, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<Campaign> GetByIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _context.Campaigns.FindAsync(userId, cancellationToken);
        }

        public async Task<List<Campaign>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Campaigns.ToListAsync(cancellationToken);
        }

        public async Task RemoveAsync(Campaign user, CancellationToken cancellationToken)
        {
            _context.Remove(user);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Campaign user, CancellationToken cancellationToken)
        {
            _context.Update(user);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<(List<Campaign> Campaigns, int TotalCount)> GetPagedAsync(
            int page,
            int pageSize,
            string? name = null,
            string? status = null,
            string? type = null,
            string? urgencyLevel = null,
            Guid? ownerId = null,
            string? description = null,
            CancellationToken cancellationToken = default)
        {
            var query = _context.Campaigns
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(c => c.Name.Contains(name));
            }

            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(c => c.Status == status);
            }

            if (!string.IsNullOrWhiteSpace(type))
            {
                query = query.Where(c => c.Type == type);
            }

            if (!string.IsNullOrWhiteSpace(urgencyLevel))
            {
                query = query.Where(c => c.UrgencyLevel == urgencyLevel);
            }

            if (ownerId.HasValue)
            {
                query = query.Where(c => c.OwnerId == ownerId.Value);
            }

            if (!string.IsNullOrWhiteSpace(description))
            {
                query = query.Where(c => 
                    (c.ShortDescription != null && c.ShortDescription.Contains(description)) ||
                    (c.FullStory != null && c.FullStory.Contains(description)));
            }

            query = query.OrderBy(c => c.CreatedDate);

            var totalCount = await query.CountAsync(cancellationToken);

            var campaigns = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (campaigns, totalCount);
        }
    }
}
