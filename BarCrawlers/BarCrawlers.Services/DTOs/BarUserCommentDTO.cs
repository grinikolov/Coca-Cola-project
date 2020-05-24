using System;
using System.Collections.Generic;
using System.Text;

namespace BarCrawlers.Services.DTOs
{
    public class BarUserCommentDTO
    {
        public Guid BarId { get; set; }
        public string BarName { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Text { get; set; }
        public bool IsFlagged { get; set; }
    }
}
