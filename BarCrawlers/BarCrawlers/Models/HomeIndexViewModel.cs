using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarCrawlers.Models
{
    public class HomeIndexViewModel
    {
       public IEnumerable<BarViewModel>  TopBars { get; set; }
       public IEnumerable<CocktailViewModel>  TopCocktails { get; set; }

    }
}
