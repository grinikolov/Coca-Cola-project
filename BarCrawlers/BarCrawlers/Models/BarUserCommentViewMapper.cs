using BarCrawlers.Models.Contracts;
using BarCrawlers.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarCrawlers.Models
{
    public class BarUserCommentViewMapper : IBarUserCommentViewMapper
    {
        public BarUserCommentView MapDTOToView(BarUserCommentDTO dto)
        {
            var view = new BarUserCommentView
            {
                BarId = dto.BarId,
                BarName = dto.BarName,
                UserId = dto.UserId,
                UserName = dto.UserName,
                Text = dto.Text,
                IsFlagged = dto.IsFlagged
            };

            return view;
        }

        public BarUserCommentDTO MapViewToDTO(BarUserCommentView view)
        {
            var dto = new BarUserCommentDTO
            {
                BarId = view.BarId,
                BarName = view.BarName,
                UserId = view.UserId,
                UserName = view.UserName,
                Text = view.Text,
                IsFlagged = view.IsFlagged
            };

            return dto;
        }
    }
}
