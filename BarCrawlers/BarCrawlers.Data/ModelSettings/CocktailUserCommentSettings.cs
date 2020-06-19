using BarCrawlers.Data.DBModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BarCrawlers.Data.ModelSettings
{
    public class CocktailUserCommentSettings : IEntityTypeConfiguration<CocktailUserComment>
    {
        public void Configure(EntityTypeBuilder<CocktailUserComment> builder)
        {
            builder.HasKey(c => new { c.CocktailId, c.UserId });
            builder.Property(c => c.Text).IsRequired();
            builder.Property(c => c.IsFlagged).IsRequired().HasDefaultValue(false);

        }
    }
}