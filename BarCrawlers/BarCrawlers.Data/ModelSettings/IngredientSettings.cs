using BarCrawlers.Data.DBModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BarCrawlers.Data.ModelSettings
{
    public class IngredientSettings : IEntityTypeConfiguration<Ingredient>
    {
        public void Configure(EntityTypeBuilder<Ingredient> builder)
        {
            builder.HasKey(i => i.Id);
            builder.HasIndex(i => i.Name).IsUnique();
            //builder.HasMany(i => i.Cocktails).WithOne(c => c.Ingredient).OnDelete(DeleteBehavior.Restrict);
            builder.Property(i => i.IsAlcoholic).IsRequired().HasDefaultValue(false);

        }
    }
}