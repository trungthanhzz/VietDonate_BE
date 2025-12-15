using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietDonate.Application.Common.Interfaces.IRepository;
using VietDonate.Application.Common.Mediator;
using VietDonate.Application.Common.Result;
using VietDonate.Application.UseCases.Auths.Commands.Logout;
using VietDonate.Application.UseCases.Auths.Commands.RefreshToken;
using VietDonate.Domain.Model.Campaigns;

namespace VietDonate.Application.UseCases.Campaigns.Queries.GetCampaign
{
    public class GetCampaignQueryHandle(ICampaignRepository _campaignRepository) : IQueryHandler<GetCampaignQuery, Result<Campaign>>
    {
        public async Task<Result<Campaign>> Handle(GetCampaignQuery query, CancellationToken cancellationToken)
        {
            var campaign = await _campaignRepository.GetByIdAsync(query.CampaignId, cancellationToken);
            if (campaign == null)
            {
                return Result.Failure<Campaign>(Error.NotFound);
            }
            return campaign;
        }
    }
}
