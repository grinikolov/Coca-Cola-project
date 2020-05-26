using BarCrawlers.Data.DBModels;
using BarCrawlers.Services.DTOs;

namespace BarCrawlers.Services.Mappers.Contracts
{
    public interface IBarCommentMapper
    {
        BarUserComment MapDTOToEntity(BarUserCommentDTO dto);
        BarUserCommentDTO MapEntityToDTO(BarUserComment entity);
    }
}