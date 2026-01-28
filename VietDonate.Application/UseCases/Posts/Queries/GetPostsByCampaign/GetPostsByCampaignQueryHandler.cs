using VietDonate.Application.Common.Interfaces.IRepository;
using VietDonate.Application.Common.Mediator;
using VietDonate.Application.Common.Result;

namespace VietDonate.Application.UseCases.Posts.Queries.GetPostsByCampaign
{
    public class GetPostsByCampaignQueryHandler(
        IPostRepository postRepository,
        ICampaignRepository campaignRepository)
        : IQueryHandler<GetPostsByCampaignQuery, Result<GetPostsByCampaignResult>>
    {
        public async Task<Result<GetPostsByCampaignResult>> Handle(
            GetPostsByCampaignQuery query,
            CancellationToken cancellationToken)
        {
            var campaign = await campaignRepository.GetByIdAsync(query.CampaignId, cancellationToken);
            
            if (campaign == null)
            {
                return Result.Failure<GetPostsByCampaignResult>(GetPostsByCampaignErrors.CampaignNotFound);
            }

            var posts = await postRepository.GetByCampaignIdAsync(query.CampaignId, cancellationToken);

            var postItems = posts.Select(p => new PostItem(
                Id: p.Id,
                Title: p.Title,
                Content: p.Content,
                PostType: p.PostType,
                Status: p.Status,
                UserId: p.UserId,
                CampaignId: p.CampaignId,
                ViewCount: p.ViewCount,
                LikeCount: p.LikeCount,
                CommentCount: p.CommentCount,
                ProofType: p.ProofType,
                ProofDate: p.ProofDate,
                CreateTime: p.CreateTime,
                UpdateTime: p.UpdateTime,
                UserName: p.User?.UserName,
                UserFullName: p.User?.UserInformation?.FullName
            )).ToList();

            var result = new GetPostsByCampaignResult(Posts: postItems);
            return Result.Success(result);
        }
    }
}
