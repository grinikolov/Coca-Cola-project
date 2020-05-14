using System;
using System.Collections.Generic;
using System.Text;

namespace BarCrawlers.Services.DTOs
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public string ImageSrc { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }


        public bool EmailConfirmed { get; set; }

        public string PhoneNumber { get; set; }

        public bool PhoneNumberConfirmed { get; set; }

        public DateTimeOffset? LockoutEnd { get; set; }

        public bool LockoutEnabled { get; set; }
    }
}
