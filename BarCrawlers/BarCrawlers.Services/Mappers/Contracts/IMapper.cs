using BarCrawlers.Data.DBModels;
using BarCrawlers.Services.DTOs;

namespace BarCrawlers.Services.Mappers.Contracts
{
    public interface ICocktailIngredientMapper
    {
        CocktailIngredient MapDTOToEntity(CocktailIngredientDTO dto);
        CocktailIngredientDTO MapEntityToDTO(CocktailIngredient entity);
    }
}