using VietDonate.Domain.Model.Comments;

namespace VietDonate.Application.Common.Interfaces.IRepository
{
    public interface ICommentRepository
    {
        Task AddAsync(Comment comment, CancellationToken cancellationToken);
        Task RemoveAsync(Comment comment, CancellationToken cancellationToken);
        Task<Comment?> GetByIdAsync(Guid commentId, CancellationToken cancellationToken);
        Task<List<Comment>> GetByPostIdAsync(Guid postId, CancellationToken cancellationToken);
        Task UpdateAsync(Comment comment, CancellationToken cancellationToken);
    }
}

