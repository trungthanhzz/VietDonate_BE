using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VietDonate.Domain.Model.CampaignCategories;

namespace VietDonate.Infrastructure.ModelInfrastructure.CampaignCategories.Persistence
{
    public class CampaignCategoryConfigurations : IEntityTypeConfiguration<CampaignCategory>
    {
        public void Configure(EntityTypeBuilder<CampaignCategory> builder)
        {
            builder.ToTable("CampaignCategories");

            builder.HasKey(cc => cc.Id);

            builder.Property(cc => cc.CampaignId)
                .IsRequired();

            builder.Property(cc => cc.CategoryId)
                .IsRequired();

            builder.Property(cc => cc.Description)
                .HasColumnType("text")
                .IsRequired(false);

            builder.Property(cc => cc.OtherInfo)
                .HasColumnType("text")
                .IsRequired(false);

            builder.Property(cc => cc.CreatedTime)
                .IsRequired();

            builder.Property(cc => cc.UpdateTime)
                .IsRequired(false);

            builder.HasOne(cc => cc.Campaign)
                .WithMany()
                .HasForeignKey(cc => cc.CampaignId)
                .HasConstraintName("FK_CampaignCategories_Campaigns_CampaignId")
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(cc => cc.Category)
                .WithMany()
                .HasForeignKey(cc => cc.CategoryId)
                .HasConstraintName("FK_CampaignCategories_Categories_CategoryId")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

