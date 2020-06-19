using BarCrawlers.Data.DBModels;
using BarCrawlers.Services.DTOs;

namespace BarCrawlers.Services.Mappers.Contracts
{
    public interface IBarUserCommentMapper
    {
        BarUserComment MapDTOToEntity(BarUserCommentDTO dto);
        BarUserCommentDTO MapEntityToDTO(BarUserComment entity);
    }
}
