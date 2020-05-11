using BarCrawlers.Data.DBModels;
using BarCrawlers.Services.DTOs;

namespace BarCrawlers.Services.Mappers.Contracts
{
    public interface IIngredientMapper 
    {
        Ingredient MapDTOToEntity(IngredientDTO dto);
        IngredientDTO MapEntityToDTO(Ingredient entity);
    }
}