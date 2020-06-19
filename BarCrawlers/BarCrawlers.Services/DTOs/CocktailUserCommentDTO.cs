using System;

namespace BarCrawlers.Services.DTOs
{
    public class CocktailUserCommentDTO
    {
        public Guid CocktailId { get; set; }
        public string CocktailName { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Text { get; set; }
        public bool IsFlagged { get; set; }
    }
}