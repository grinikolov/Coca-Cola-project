using BarCrawlers.Data.DBModels;
using BarCrawlers.Services.DTOs;
using BarCrawlers.Services.Mappers.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BarCrawlers.Services.Mappers
{
    public class CocktailIngredientMapper : ICocktailIngredientMapper
    {
        public CocktailIngredient MapDTOToEntity(CocktailIngredientDTO dto)
        {
            return new CocktailIngredient
            {
                CocktailId = dto.CocktailId,
               // Cocktail = dto.Cocktail,
                IngredientId = dto.IngredientId,
                //Ingredient = dto.Ingredient,
                Parts = dto.Parts,
            };
        }

        public CocktailIngredientDTO MapEntityToDTO(CocktailIngredient entity)
        {
            return new CocktailIngredientDTO
            {
                CocktailId = entity.CocktailId,
                //Cocktail = entity.Cocktail,
                IngredientId = entity.IngredientId,
                //Ingredient = entity.Ingredient,
                Parts = entity.Parts,
            };
        }
    }
}
