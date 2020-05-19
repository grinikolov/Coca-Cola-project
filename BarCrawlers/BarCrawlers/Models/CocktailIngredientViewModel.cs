using BarCrawlers.Services.DTOs;
using System;
using System.Collections.Generic;

namespace BarCrawlers.Models
{
    public class CocktailIngredientViewModel
    {    
        public Guid IngredientId { get; set; }
        public string IngredientName { get; set; }
        public int? Parts { get; set; }

    }
}