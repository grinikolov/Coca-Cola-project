using System;
using System.Collections.Generic;
using System.Text;

namespace BarCrawlers.Data.DBModels
{
    public class Cocktail
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Rating { get; set; }
        public int TimesRated { get; set; }
        public string ImageSrc { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsAlcoholic { get; set; }
        public ICollection<CocktailIngredient> Ingredients { get; set; }
        public ICollection<CocktailBar> Bars { get; set; }
        public ICollection<CocktailUserComment> Comment { get; set; }
        public ICollection<UserCocktailRating> CocktailRatings { get; set; }
        public string Instructions { get; set; }

    }
}
