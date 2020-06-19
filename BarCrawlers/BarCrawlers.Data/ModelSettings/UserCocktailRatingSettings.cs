using BarCrawlers.Data.DBModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BarCrawlers.Data.ModelSettings
{
    public class UserCocktailRatingSettings : IEntityTypeConfiguration<UserCocktailRating>
    {
        public void Configure(EntityTypeBuilder<UserCocktailRating> builder)
        {
            builder.HasKey(b => new { b.CocktailId, b.UserId });
            builder.Property(c => c.Rating).IsRequired();

        }
    }
}