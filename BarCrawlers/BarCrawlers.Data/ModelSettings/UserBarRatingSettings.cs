using BarCrawlers.Data.DBModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BarCrawlers.Data.ModelSettings
{
    public class UserBarRatingSettings : IEntityTypeConfiguration<UserBarRating>
    {
        public void Configure(EntityTypeBuilder<UserBarRating> builder)
        {
            builder.HasKey(b => new { b.BarId, b.UserId });
            builder.Property(c => c.Rating).IsRequired();

        }
    }
}