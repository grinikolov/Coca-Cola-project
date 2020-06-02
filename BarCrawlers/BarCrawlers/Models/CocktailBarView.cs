using System;

namespace BarCrawlers.Models
{
    public class CocktailBarView
    {
        public Guid CocktailId { get; set; }

        public string CocktailName { get; set; }

        public Guid BarId { get; set; }

        public string BarName { get; set; }

        public bool Remove { get; set; }
    }
}