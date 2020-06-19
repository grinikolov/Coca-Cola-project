using System;
using System.Collections.Generic;

namespace BarCrawlers.Services.DTOs
{
    public class CocktailDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Rating { get; set; }
        public int TimesRated { get; set; }
        public string ImageSrc { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsAlcoholic { get; set; }
        public ICollection<CocktailIngredientDTO> Ingredients { get; set; }
        public ICollection<CocktailBarDTO> Bars { get; set; }
        public ICollection<CocktailUserCommentDTO> Comments { get; set; }
        public ICollection<UserCocktailRatingDTO> CocktailRatings { get; set; }
        public string Instructions { get; set; }

    }
}
