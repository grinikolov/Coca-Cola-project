using BarCrawlers.Data.DBModels;
using BarCrawlers.Services.DTOs;
using BarCrawlers.Services.Mappers.Contracts;
using System;
using System.Linq;

namespace BarCrawlers.Services.Mappers
{
    public class IngredientMapper : IIngredientMapper //IEntityMapper<Ingredient, IngredientDTO>, IDTOMapper<IngredientDTO, Ingredient>, IIngredientMapper
    {
        public Ingredient MapDTOToEntity(IngredientDTO dto)
        {
            try
            {
                return new Ingredient
                {
                    Id = dto.Id,
                    Name = dto.Name,
                    IsAlcoholic = dto.IsAlcoholic,

                    //TODO: Mapping list of cocktails?
                    //Cocktails = dto.Cocktails.Select(c => c.CocktailId)
                };
            }
            catch (Exception)
            {
                return new Ingredient();
            }
        }

        public IngredientDTO MapEntityToDTO(Ingredient entity)
        {
            try
            {
                return new IngredientDTO
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    IsAlcoholic = entity.IsAlcoholic,
                    Cocktails = entity.Cocktails.Select(c => new CocktailIngredientDTO()
                    {
                        IngredientId = entity.Id,
                        IngredientName = entity.Name,
                        CocktailId = c.CocktailId,
                        CocktailName = c.Cocktail.Name,
                        Parts = c.Parts
                    }).ToList(),
                };
            }
            catch (Exception)
            {
                return new IngredientDTO();
            }
        }
    }
}
