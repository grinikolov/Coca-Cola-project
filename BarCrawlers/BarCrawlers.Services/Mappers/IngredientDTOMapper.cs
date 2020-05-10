using BarCrawlers.Data.DBModels;
using BarCrawlers.Services.DTOs;
using BarCrawlers.Services.Mappers.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace BarCrawlers.Services.Mappers
{
    /* TODO: This Should not exist if its in the other mapper:
    class IngredientDTOMapper : IEntityMapper<IngredientDTO, Ingredient>
    {
        public Ingredient MapDTOToEntity(IngredientDTO dto)
        {
            return new Ingredient
            {
                Id = dto.Id,
                Name = dto.Name,
                IsAlcoholic = dto.IsAlcoholic,

                //TODO: Mapping list of cocktails?
                //Cocktails = dto.Cocktails
            };
        }
    }
    */
}
