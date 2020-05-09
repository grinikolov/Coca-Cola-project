using BarCrawlers.Data.DBModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BarCrawlers.Data.ModelSettings
{
    public class CocktailSettings : IEntityTypeConfiguration<Cocktail>
    {
        public void Configure(EntityTypeBuilder<Cocktail> builder)
        {
            builder.HasKey(c => c.Id);
            builder.HasIndex(c => c.Name).IsUnique();
            //builder.HasMany(i => i.Cocktails).WithOne(c => c.Ingredient).OnDelete(DeleteBehavior.Restrict);
            builder.Property(c => c.IsAlcoholic).IsRequired().HasDefaultValue(false);
            builder.Property(c => c.IsDeleted).IsRequired().HasDefaultValue(false);
            builder.Property(c => c.Rating).IsRequired().HasDefaultValue(0);
            builder.Property(c => c.TimesRated).IsRequired().HasDefaultValue(0);
            builder.Property(c => c.Instructions).IsRequired().HasDefaultValue("Mix all ingredients.");
    }
    }
}