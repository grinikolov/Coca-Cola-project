using BarCrawlers.Data.DBModels;
using BarCrawlers.Services.DTOs;
using BarCrawlers.Services.Mappers.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace BarCrawlers.Services.Mappers
{
    public class BarUserCommentMapper : IBarUserCommentMapper
    {
        public BarUserComment MapDTOToEntity(BarUserCommentDTO dto)
        {
            var entity = new BarUserComment
            {
                BarId = dto.BarId,
                UserId = dto.UserId,
                Text = dto.Text,
                IsFlagged = dto.IsFlagged
            };

            return entity;
        }

        public BarUserCommentDTO MapEntityToDTO(BarUserComment entity)
        {
            var dto = new BarUserCommentDTO
            {
                BarId = entity.BarId,
                BarName = entity.Bar.Name,
                UserId = entity.UserId,
                UserName = entity.User.UserName,
                Text = entity.Text,
                IsFlagged = entity.IsFlagged
            };

            return dto;
        }
    }
}
