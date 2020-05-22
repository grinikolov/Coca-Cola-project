using BarCrawlers.Services.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BarCrawlers.Models
{
    public class CocktailCreateViewModel
    {
        public Guid Id { get; set; }
        [StringLength(70, MinimumLength = 3)]
        public string Name { get; set; }
        public double Rating { get; set; }
        public int TimesRated { get; set; }
        public string ImageSrc { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsAlcoholic { get; set; }

        [Required(ErrorMessage = "Choose at least one ingredient!")]
        public ICollection<CocktailIngredientViewModel> Ingredients { get; set; }
        //public ICollection<CocktailBarDTO> Bars { get; set; }
        //public ICollection<CocktailUserCommentDTO> Comments { get; set; }
        //public ICollection<UserCocktailRatingDTO> CocktailRatings { get; set; }
        [StringLength(500)]
        public string Instructions { get; set; }

    }
}