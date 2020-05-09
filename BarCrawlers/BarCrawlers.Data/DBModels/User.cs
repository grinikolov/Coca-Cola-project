using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BarCrawlers.Data.DBModels
{
    public class User : IdentityUser<Guid>
    {

        public string ImageSrc { get; set; }

        public ICollection<BarUserComment> BarComments { get; set; }
        public ICollection<CocktailUserComment> CocktailComments { get; set; }

        public ICollection<UserBarRating> BarRatings { get; set; }
        public ICollection<UserCocktailRating> CocktailRatings { get; set; }


    }
}
