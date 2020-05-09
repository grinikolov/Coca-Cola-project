using System;
using System.Collections.Generic;
using System.Text;

namespace BarCrawlers.Data.DBModels
{
    public class UserBarRating
    {
        public Guid BarId { get; set; }
        public Bar Bar { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public int Rating { get; set; }
    }
}
