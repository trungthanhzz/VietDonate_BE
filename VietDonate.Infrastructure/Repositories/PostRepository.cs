using Microsoft.EntityFrameworkCore;
using VietDonate.Application.Common.Interfaces.IRepository;
using VietDonate.Domain.Model.Posts;
using VietDonate.Infrastructure.Common.Persistance;

namespace VietDonate.Infrastructure.Repositories
{
    public class PostRepository(AppDbContext context) : IPostRepository
    {
        public async Task AddAsync(Post post, CancellationToken cancellationToken)
        {
            await context.Posts.AddAsync(post, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task<Post?> GetByIdAsync(Guid postId, CancellationToken cancellationToken)
        {
            return await context.Posts
                .Include(p => p.User)
                .Include(p => p.Campaign)
                .FirstOrDefaultAsync(p => p.Id == postId, cancellationToken);
        }

        public async Task<List<Post>> GetByCampaignIdAsync(Guid campaignId, CancellationToken cancellationToken)
        {
            return await context.Posts
                .Include(p => p.User)
                .Include(p => p.Campaign)
                .Where(p => p.CampaignId == campaignId)
                .OrderByDescending(p => p.CreateTime)
                .ToListAsync(cancellationToken);
        }

        public async Task RemoveAsync(Post post, CancellationToken cancellationToken)
        {
            context.Posts.Remove(post);
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Post post, CancellationToken cancellationToken)
        {
            context.Posts.Update(post);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
