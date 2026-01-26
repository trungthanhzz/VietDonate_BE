using VietDonate.Application.Common.Interfaces.IRepository;
using VietDonate.Application.Common.Mediator;
using VietDonate.Application.Common.Result;

namespace VietDonate.Application.UseCases.Campaigns.Queries.GetCampaigns
{
    public class GetCampaignsQueryHandler(
        ICampaignRepository campaignRepository)
        : IQueryHandler<GetCampaignsQuery, Result<GetCampaignsResult>>
    {
        private const int MaxPageSize = 100;
        private const int DefaultPageSize = 20;

        public async Task<Result<GetCampaignsResult>> Handle(
            GetCampaignsQuery query,
            CancellationToken cancellationToken)
        {
            // Validate page number
            if (query.Page < 1)
            {
                return Result.Failure<GetCampaignsResult>(GetCampaignsErrors.InvalidPageNumber);
            }

            // Validate and normalize page size
            var pageSize = query.PageSize;
            if (pageSize < 1)
            {
                pageSize = DefaultPageSize;
            }
            else if (pageSize > MaxPageSize)
            {
                return Result.Failure<GetCampaignsResult>(GetCampaignsErrors.InvalidPageSize);
            }

            // Get paginated campaigns with filters
            var (campaigns, totalCount) = await campaignRepository.GetPagedAsync(
                query.Page,
                pageSize,
                query.Name,
                query.Status,
                query.Type,
                query.UrgencyLevel,
                query.OwnerId,
                query.Description,
                cancellationToken);

            // Calculate pagination metadata
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            var hasPreviousPage = query.Page > 1;
            var hasNextPage = query.Page < totalPages;

            // Map to result items
            var campaignItems = campaigns.Select(c => new CampaignItem(
                Id: c.Id,
                Code: c.Code,
                Name: c.Name,
                CreatedDate: c.CreatedDate,
                ShortDescription: c.ShortDescription,
                TargetAmount: c.TargetAmount,
                CurrentAmount: c.CurrentAmount,
                Type: c.Type,
                UrgencyLevel: c.UrgencyLevel,
                Status: c.Status,
                ViewCount: c.ViewCount,
                DonorCount: c.DonorCount,
                OwnerId: c.OwnerId
            )).ToList();

            var paginationMetadata = new PaginationMetadata(
                Page: query.Page,
                PageSize: pageSize,
                TotalCount: totalCount,
                TotalPages: totalPages,
                HasPreviousPage: hasPreviousPage,
                HasNextPage: hasNextPage
            );

            var result = new GetCampaignsResult(
                Campaigns: campaignItems,
                Pagination: paginationMetadata
            );

            return Result.Success(result);
        }
    }
}
