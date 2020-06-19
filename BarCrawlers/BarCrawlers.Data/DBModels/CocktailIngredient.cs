using System;

namespace BarCrawlers.Data.DBModels
{
    public class CocktailIngredient
    {
        public Guid CocktailId { get; set; }
        public Cocktail Cocktail { get; set; }
        public Guid IngredientId { get; set; }
        public Ingredient Ingredient { get; set; }
        public int? Parts { get; set; }
    }
}
