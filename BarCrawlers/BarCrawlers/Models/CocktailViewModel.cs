using BarCrawlers.Services.DTOs;
using System;
using System.Collections.Generic;

namespace BarCrawlers.Models
{
    public class CocktailViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Rating { get; set; }
        public int TimesRated { get; set; }
        public string ImageSrc { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsAlcoholic { get; set; }
        public ICollection<CocktailIngredientViewModel> Ingredients { get; set; }
        public ICollection<CocktailBarView> Bars { get; set; }
        public ICollection<CocktailUserCommentVM> Comments { get; set; }
        public ICollection<UserCocktailRatingDTO> CocktailRatings { get; set; }
        public string Instructions { get; set; }

    }
}