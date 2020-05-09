using System;
using System.Collections;

namespace BarCrawlers.Data.DBModels
{
    public class CocktailUserComment
    {
        public Guid CocktailId { get; set; }
        public Cocktail Cocktail { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public string Text { get; set; }
        public bool IsFlagged { get; set; }
    }
}