using System;
using System.ComponentModel.DataAnnotations;

namespace BarCrawlers.Models
{
    public class BarUserCommentView
    {
        public Guid BarId { get; set; }
        public string BarName { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        [MaxLength(500)]
        public string Text { get; set; }
        public bool IsFlagged { get; set; }
    }
}