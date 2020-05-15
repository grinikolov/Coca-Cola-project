using System;

namespace BarCrawlers.Services.DTOs
{
    public class CocktailIngredientDTO
    {
        public Guid CocktailId { get; set; }
        public string CocktailName { get; set; }
        //public CocktailDTO Cocktail { get; set; }
        public Guid IngredientId { get; set; }
        public string IngredientName { get; set; }
        //public IngredientDTO Ingredient { get; set; }
        public int? Parts { get; set; }
    }
}