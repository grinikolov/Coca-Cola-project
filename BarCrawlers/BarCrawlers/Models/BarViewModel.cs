using System;
using System.Collections.Generic;

namespace BarCrawlers.Models
{
    public class BarViewModel
    {
        public BarViewModel()
        {
            Cocktails = new List<CocktailBarView>();
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Rating { get; set; }
        public int TimesRated { get; set; }
        public string ImageSrc { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<CocktailBarView> Cocktails { get; set; }
        public ICollection<BarUserCommentView> Comments { get; set; }
        public ICollection<UserBarRatingView> BarRatings { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string District { get; set; }
        public string Town { get; set; }
        public string Country { get; set; }

        public Guid? LocationId { get; set; }
        public LocationView Location { get; set; }
    }
}
