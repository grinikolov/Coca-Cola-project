using BarCrawlers.Data.DBModels;
using BarCrawlers.Services.DTOs;
using BarCrawlers.Services.Mappers.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BarCrawlers.Services.Mappers
{
    public class IngredientMapper : IIngredientMapper //IEntityMapper<Ingredient, IngredientDTO>, IDTOMapper<IngredientDTO, Ingredient>, IIngredientMapper
    {
        public Ingredient MapDTOToEntity(IngredientDTO dto)
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

        public IngredientDTO MapEntityToDTO(Ingredient entity)
        {
            return new IngredientDTO
            {
                Id = entity.Id,
                Name = entity.Name,
                IsAlcoholic = entity.IsAlcoholic,

                //TODO: What about coctails mapping?
                //Cocktails = entity.Cocktails.Select(c => CocktailIngredientMapper(c)),
            };
        }
    }
}
