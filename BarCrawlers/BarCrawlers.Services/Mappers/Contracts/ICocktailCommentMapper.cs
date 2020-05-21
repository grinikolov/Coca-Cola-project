using BarCrawlers.Data.DBModels;
using BarCrawlers.Services.DTOs;

namespace BarCrawlers.Services.Mappers.Contracts
{
    public interface ICocktailCommentMapper
    {
        CocktailUserComment MapDTOToEntity(CocktailUserCommentDTO dto);
        CocktailUserCommentDTO MapEntityToDTO(CocktailUserComment entity);
    }
}