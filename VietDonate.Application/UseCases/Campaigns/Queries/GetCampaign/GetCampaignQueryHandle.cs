using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietDonate.Application.Common.Interfaces.IRepository;
using VietDonate.Application.Common.Mediator;
using VietDonate.Application.Common.Result;
using VietDonate.Domain.Model.Campaigns;

namespace VietDonate.Application.UseCases.Campaigns.Queries.GetCampaign
{
    public class GetCampaignQueryHandle(ICampaignRepository campaignRepository) : IQueryHandler<GetCampaignQuery, Result<Campaign>>
    {
        public Task<Result<Campaign>> Handle(GetCampaignQuery query, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
