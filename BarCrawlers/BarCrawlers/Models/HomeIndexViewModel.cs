using System.Collections.Generic;

namespace BarCrawlers.Models
{
    public class HomeIndexViewModel
    {
        public IEnumerable<BarViewModel> TopBars { get; set; }
        public IEnumerable<CocktailViewModel> TopCocktails { get; set; }

    }
}
