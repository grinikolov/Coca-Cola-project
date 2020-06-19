using BarCrawlers.Data.DBModels;
using BarCrawlers.Services.DTOs;

namespace BarCrawlers.Services.Mappers.Contracts
{
    public interface ICocktailMapper
    {
        Cocktail MapDTOToEntity(CocktailDTO dto);
        CocktailDTO MapEntityToDTO(Cocktail entity);
    }
}