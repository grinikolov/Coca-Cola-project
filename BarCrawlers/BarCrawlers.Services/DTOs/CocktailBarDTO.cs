using System;

namespace BarCrawlers.Services.DTOs
{
    public class CocktailBarDTO
    {
        public Guid BarId { get; set; }
        public string BarName { get; set; }
        public Guid CocktailId { get; set; }
        public string CocktailName { get; set; }
        public bool Remove { get; set; }
    }
}