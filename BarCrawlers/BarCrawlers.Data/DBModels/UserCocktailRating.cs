using System;

namespace BarCrawlers.Data.DBModels
{
    public class UserCocktailRating
    {
        public Guid CocktailId { get; set; }
        public Cocktail Cocktail { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public int Rating { get; set; }
    }
}
