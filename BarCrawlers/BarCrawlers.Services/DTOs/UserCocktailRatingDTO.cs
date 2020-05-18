using System;
using System.Collections.Generic;
using System.Text;

namespace BarCrawlers.Services.DTOs
{
    public class UserCocktailRatingDTO
    {
        public Guid CocktailId { get; set; }
        public string CocktailName { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public int Rating { get; set; }
    }
}
