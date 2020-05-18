using System;

namespace BarCrawlers.Data.DBModels
{
    public class CocktailBar
    {
        public Guid BarId { get; set; }
        public Bar Bar { get; set; }
        public Guid CocktailId { get; set; }
        public Cocktail Cocktail { get; set; }
    }
}