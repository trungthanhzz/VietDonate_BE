using VietDonate.Domain.Common;
using VietDonate.Domain.Model.Media;

namespace VietDonate.Application.Common.Interfaces.IRepository
{
    public interface IMediaRepository
    {
        Task<Media?> GetByIdAsync(Guid mediaId, CancellationToken cancellationToken);
        Task<List<Media>> GetByCampaignIdAsync(Guid campaignId, CancellationToken cancellationToken);
        Task<List<Media>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken);
        Task AddAsync(Media media, CancellationToken cancellationToken);
        Task UpdateAsync(Media media, CancellationToken cancellationToken);
        Task DeleteAsync(Media media, CancellationToken cancellationToken);
        Task<bool> ExistsAsync(Guid mediaId, CancellationToken cancellationToken);
    }
}
