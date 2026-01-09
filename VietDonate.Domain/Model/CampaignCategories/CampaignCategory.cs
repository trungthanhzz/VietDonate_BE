using VietDonate.Domain.Common;
using VietDonate.Domain.Model.Campaigns;
using VietDonate.Domain.Model.Categories;

namespace VietDonate.Domain.Model.CampaignCategories
{
    public class CampaignCategory : Entity
    {
        public Guid CampaignId { get; set; }
        public Guid CategoryId { get; set; }
        public string? Description { get; set; }
        public string? OtherInfo { get; set; }
        public DateTime CreatedTime { get; set; } = DateTime.UtcNow;
        public DateTime? UpdateTime { get; set; }

        public Campaign Campaign { get; set; } = null!;
        public Category Category { get; set; } = null!;

        public CampaignCategory(Guid id, Guid campaignId, Guid categoryId) : base(id)
        {
            CampaignId = campaignId;
            CategoryId = categoryId;
            CreatedTime = DateTime.UtcNow;
        }

        private CampaignCategory()
        {
        }
    }
}

