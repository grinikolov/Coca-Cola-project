using System;
using System.Collections.Generic;

namespace BarCrawlers.Services.DTOs
{
    public class IngredientDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsAlcoholic { get; set; }
        //public ICollection<CocktailIngredientDTO???> Cocktails { get; set; }
    }
}