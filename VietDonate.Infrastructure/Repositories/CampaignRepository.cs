using Microsoft.EntityFrameworkCore;
using VietDonate.Application.Common.Interfaces.IRepository;
using VietDonate.Domain.Model.Campaigns;
using VietDonate.Infrastructure.Common.Persistance;
using System.Linq.Expressions;

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
            return await _context.Campaigns
                .Include(c => c.OwnerUser)
                    .ThenInclude(u => u.UserInformation)
                .FirstOrDefaultAsync(c => c.Id == userId, cancellationToken);
        }

        public async Task<List<Campaign>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Campaigns
                .Include(c => c.OwnerUser)
                    .ThenInclude(u => u.UserInformation)
                .ToListAsync(cancellationToken);
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
                .Include(c => c.OwnerUser)
                    .ThenInclude(u => u.UserInformation)
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

        public async Task<List<Campaign>> GetTopByEndTimeAsync(
            int count,
            CancellationToken cancellationToken = default)
        {
            return await _context.Campaigns
                .AsNoTracking()
                .Include(c => c.OwnerUser)
                    .ThenInclude(u => u.UserInformation)
                .Where(c =>
                    c.Status == "Approved" &&
                    c.EndTime != null)
                .OrderByDescending(c => c.EndTime)
                .ThenByDescending(c => c.CreatedDate)
                .Take(count)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<T>> GetTopByEndTimeAsync<T>(
            int count,
            Expression<Func<Campaign, T>> selector,
            CancellationToken cancellationToken = default)
        {
            return await _context.Campaigns
                .AsNoTracking()
                .Where(c =>
                    c.Status == "Approved" &&
                    c.EndTime != null)
                .OrderByDescending(c => c.EndTime)
                .ThenByDescending(c => c.CreatedDate)
                .Take(count)
                .Select(selector)
                .ToListAsync(cancellationToken);
        }
    }
}
