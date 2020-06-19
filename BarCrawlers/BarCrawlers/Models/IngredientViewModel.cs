using System;

namespace BarCrawlers.Models
{
    public class IngredientViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsAlcoholic { get; set; }
    }
}
