using VietDonate.Domain.Model.Posts;

namespace VietDonate.Application.Common.Interfaces.IRepository
{
    public interface IPostRepository
    {
        Task AddAsync(Post post, CancellationToken cancellationToken);
        Task<Post?> GetByIdAsync(Guid postId, CancellationToken cancellationToken);
        Task<List<Post>> GetByCampaignIdAsync(Guid campaignId, CancellationToken cancellationToken);
        Task RemoveAsync(Post post, CancellationToken cancellationToken);
        Task UpdateAsync(Post post, CancellationToken cancellationToken);
    }
}
