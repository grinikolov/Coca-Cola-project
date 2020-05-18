using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarCrawlers.Areas.Magician.Models
{
    public class UserViewModel
    {
        public Guid Id { get; set; }
        public string ImageSrc { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public bool Locked { get; set; }

        public bool EmailConfirmed { get; set; }

        public string Role { get; set; }

        public string PhoneNumber { get; set; }

        public bool PhoneNumberConfirmed { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }

        public bool LockoutEnabled { get; set; }

        public int LockDays { get; set; }

        //public ICollection<BarUserComment> BarComments { get; set; }
        //public ICollection<CocktailUserComment> CocktailComments { get; set; }

        //public ICollection<UserBarRating> BarRatings { get; set; }
        //public ICollection<UserCocktailRating> CocktailRatings { get; set; }
    }
}
