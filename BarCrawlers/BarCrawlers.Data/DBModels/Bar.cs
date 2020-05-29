using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace BarCrawlers.Data.DBModels
{
    public class Bar
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Rating { get; set; }
        public int TimesRated { get; set; }
        public string ImageSrc { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<CocktailBar> Cocktails { get; set; }
        public ICollection<BarUserComment> Comments { get; set; }
        public ICollection<UserBarRating> BarRatings { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string District { get; set; }
        public string Town { get; set; }
        public string Country { get; set; }

        public Guid? LocationId { get; set; }
        public Location Location { get; set; }
    }
}
