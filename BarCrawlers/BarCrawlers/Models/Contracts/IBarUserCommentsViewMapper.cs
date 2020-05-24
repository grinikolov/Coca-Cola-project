using BarCrawlers.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarCrawlers.Models.Contracts
{
    public interface IBarUserCommentViewMapper
    {
        public BarUserCommentView MapDTOToView(BarUserCommentDTO dto);

        public BarUserCommentDTO MapViewToDTO(BarUserCommentView view);
    }
}
