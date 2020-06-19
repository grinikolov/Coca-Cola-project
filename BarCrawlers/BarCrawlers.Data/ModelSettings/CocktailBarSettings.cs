using BarCrawlers.Data.DBModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BarCrawlers.Data.ModelSettings
{
    public class CocktailBarSettings : IEntityTypeConfiguration<CocktailBar>
    {
        public void Configure(EntityTypeBuilder<CocktailBar> builder)
        {
            builder.HasKey(c => new { c.BarId, c.CocktailId });
        }
    }
}