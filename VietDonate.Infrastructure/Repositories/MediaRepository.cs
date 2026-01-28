using Microsoft.EntityFrameworkCore;
using VietDonate.Application.Common.Interfaces.IRepository;
using VietDonate.Domain.Model.Media;
using VietDonate.Infrastructure.Common.Persistance;

namespace VietDonate.Infrastructure.Repositories
{
    public class MediaRepository(AppDbContext context) : IMediaRepository
    {
        public async Task<Media?> GetByIdAsync(Guid mediaId, CancellationToken cancellationToken)
        {
            return await context.Media
                .FirstOrDefaultAsync(m => m.Id == mediaId, cancellationToken);
        }

        public async Task<List<Media>> GetByCampaignIdAsync(Guid campaignId, CancellationToken cancellationToken)
        {
            return await context.Media
                .Where(m => m.CampaignId == campaignId)
                .OrderBy(m => m.DisplayOrder)
                .ThenBy(m => m.CreateTime)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Media>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await context.Media
                .Where(m => m.UserId == userId)
                .OrderBy(m => m.CreateTime)
                .ToListAsync(cancellationToken);
        }

        public async Task AddAsync(Media media, CancellationToken cancellationToken)
        {
            await context.Media.AddAsync(media, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Media media, CancellationToken cancellationToken)
        {
            context.Media.Update(media);
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Media media, CancellationToken cancellationToken)
        {
            context.Media.Remove(media);
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> ExistsAsync(Guid mediaId, CancellationToken cancellationToken)
        {
            return await context.Media
                .AnyAsync(m => m.Id == mediaId, cancellationToken);
        }
    }
}
