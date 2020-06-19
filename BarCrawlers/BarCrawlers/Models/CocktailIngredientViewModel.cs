using System;

namespace BarCrawlers.Models
{
    public class CocktailIngredientViewModel
    {
        public Guid IngredientId { get; set; }
        public string IngredientName { get; set; }
        public int? Parts { get; set; }

    }
}