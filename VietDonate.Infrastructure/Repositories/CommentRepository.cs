using Microsoft.EntityFrameworkCore;
using VietDonate.Application.Common.Interfaces.IRepository;
using VietDonate.Domain.Model.Comments;
using VietDonate.Infrastructure.Common.Persistance;

namespace VietDonate.Infrastructure.Repositories
{
    public class CommentRepository(AppDbContext context) : ICommentRepository
    {
        public async Task AddAsync(Comment comment, CancellationToken cancellationToken)
        {
            await context.Comments.AddAsync(comment, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task RemoveAsync(Comment comment, CancellationToken cancellationToken)
        {
            context.Comments.Remove(comment);
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task<Comment?> GetByIdAsync(Guid commentId, CancellationToken cancellationToken)
        {
            return await context.Comments
                .Include(c => c.User)
                .ThenInclude(u => u.UserInformation)
                .FirstOrDefaultAsync(c => c.Id == commentId, cancellationToken);
        }

        public async Task<List<Comment>> GetByPostIdAsync(Guid postId, CancellationToken cancellationToken)
        {
            return await context.Comments
                .AsNoTracking()
                .Include(c => c.User)
                .ThenInclude(u => u.UserInformation)
                .Where(c => c.PostId == postId && c.ParentId == null)
                .OrderBy(c => c.CreateTime)
                .ToListAsync(cancellationToken);
        }

        public async Task UpdateAsync(Comment comment, CancellationToken cancellationToken)
        {
            context.Comments.Update(comment);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}

