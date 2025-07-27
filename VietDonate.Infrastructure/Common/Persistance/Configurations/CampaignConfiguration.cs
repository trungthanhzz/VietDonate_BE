using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VietDonate.Domain.Campaigns;

namespace VietDonate.Infrastructure.Common.Persistance.Configurations
{
    public class CampaignConfiguration : IEntityTypeConfiguration<Campaign>
    {
        public void Configure(EntityTypeBuilder<Campaign> builder)
        {
            builder.ToTable("Campaigns");
            
            builder.HasKey(c => c.Id);
            
            builder.Property(c => c.Id)
                .HasColumnType("uuid")
                .IsRequired();
            
            builder.Property(c => c.CreatedDate)
                .HasColumnType("timestamp without time zone")
                .IsRequired();
        }
    }
} 