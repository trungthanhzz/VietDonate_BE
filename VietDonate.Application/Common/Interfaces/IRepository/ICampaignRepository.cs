using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietDonate.Domain.Model.Campaigns;

namespace VietDonate.Application.Common.Interfaces.IRepository
{
    public interface ICampaignRepository
    {
        Task AddAsync(Campaign user, CancellationToken cancellationToken);
        Task<Campaign> GetByIdAsync(Guid userId, CancellationToken cancellationToken);
        Task<List<Campaign>> GetAllAsync(CancellationToken cancellationToken);
        Task RemoveAsync(Campaign user, CancellationToken cancellationToken);
        Task UpdateAsync(Campaign user, CancellationToken cancellationToken);
        Task<(List<Campaign> Campaigns, int TotalCount)> GetPagedAsync(
            int page,
            int pageSize,
            string? name = null,
            string? status = null,
            string? type = null,
            string? urgencyLevel = null,
            Guid? ownerId = null,
            string? description = null,
            CancellationToken cancellationToken = default);
    }
}
