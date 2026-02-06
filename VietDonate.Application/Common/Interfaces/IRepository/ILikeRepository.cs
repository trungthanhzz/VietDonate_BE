using VietDonate.Domain.Model.Likes;

namespace VietDonate.Application.Common.Interfaces.IRepository
{
    public interface ILikeRepository
    {
        Task AddAsync(Like like, CancellationToken cancellationToken);
        Task RemoveAsync(Like like, CancellationToken cancellationToken);
        Task<List<Like>> GetByPostIdAsync(Guid postId, CancellationToken cancellationToken);
        Task<Like?> GetByUserAndPostAsync(Guid userId, Guid postId, CancellationToken cancellationToken);
    }
}

