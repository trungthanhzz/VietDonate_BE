using Microsoft.EntityFrameworkCore;
using VietDonate.Application.Common.Interfaces.IRepository;
using VietDonate.Domain.Model.Likes;
using VietDonate.Infrastructure.Common.Persistance;

namespace VietDonate.Infrastructure.Repositories
{
    public class LikeRepository(AppDbContext context) : ILikeRepository
    {
        public async Task AddAsync(Like like, CancellationToken cancellationToken)
        {
            await context.Likes.AddAsync(like, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task RemoveAsync(Like like, CancellationToken cancellationToken)
        {
            context.Likes.Remove(like);
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<Like>> GetByPostIdAsync(Guid postId, CancellationToken cancellationToken)
        {
            return await context.Likes
                .AsNoTracking()
                .Include(l => l.User)
                .Where(l => l.PostId == postId)
                .OrderByDescending(l => l.CreatedTime)
                .ToListAsync(cancellationToken);
        }

        public async Task<Like?> GetByUserAndPostAsync(Guid userId, Guid postId, CancellationToken cancellationToken)
        {
            return await context.Likes
                .FirstOrDefaultAsync(l => l.UserId == userId && l.PostId == postId, cancellationToken);
        }
    }
}

