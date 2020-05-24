using BarCrawlers.Data.DBModels;
using BarCrawlers.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace BarCrawlers.Services.Mappers.Contracts
{
    public interface IBarUserCommentMapper
    {
        BarUserComment MapDTOToEntity(BarUserCommentDTO dto);
        BarUserCommentDTO MapEntityToDTO(BarUserComment entity);
    }
}
