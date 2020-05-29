using System;
using System.Collections.Generic;
using System.Text;

namespace BarCrawlers.Services.DTOs
{
    public class BarDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Rating { get; set; }
        public int TimesRated { get; set; }
        public string ImageSrc { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<CocktailBarDTO> Cocktails { get; set; }
        public ICollection<BarUserCommentDTO> Comments { get; set; }
        public ICollection<UserBarRatingDTO> BarRatings { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string District { get; set; }
        public string Town { get; set; }
        public string Country { get; set; }

        public Guid? LocationId { get; set; }
        public LocationDTO Location { get; set; }
    }
}
