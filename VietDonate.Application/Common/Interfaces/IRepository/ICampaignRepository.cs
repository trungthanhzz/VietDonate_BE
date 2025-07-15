using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietDonate.Domain.Campaigns;

namespace VietDonate.Application.Common.Interfaces.IRepository
{
    public interface ICampaignRepository
    {
        Task AddAsync(Campaign user, CancellationToken cancellationToken);
        Task<Campaign> GetByIdAsync(Guid userId, CancellationToken cancellationToken);
        Task RemoveAsync(Campaign user, CancellationToken cancellationToken);
        Task UpdateAsync(Campaign user, CancellationToken cancellationToken);
    }
}
