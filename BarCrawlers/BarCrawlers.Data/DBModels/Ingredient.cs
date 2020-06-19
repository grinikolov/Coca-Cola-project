using System;
using System.Collections.Generic;

namespace BarCrawlers.Data.DBModels
{
    public class Ingredient
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsAlcoholic { get; set; }
        public ICollection<CocktailIngredient> Cocktails { get; set; }
    }
}
