using System;

namespace BarCrawlers.Data.DBModels
{
    public class BarUserComment
    {
        public Guid BarId { get; set; }
        public Bar Bar { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public string Text { get; set; }
        public bool IsFlagged { get; set; }
    }
}