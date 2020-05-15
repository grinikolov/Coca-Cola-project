using BarCrawlers.Data.DBModels;
using BarCrawlers.Services.DTOs;
using BarCrawlers.Services.Mappers.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BarCrawlers.Services.Mappers
{
    public class CocktailMapper : ICocktailMapper
    {
        public Cocktail MapDTOToEntity(CocktailDTO dto)
        {
            return new Cocktail
            {
                Id = dto.Id,
                Name = dto.Name,
                Rating = dto.Rating,
                TimesRated = dto.TimesRated,
                ImageSrc = dto.ImageSrc,
                IsDeleted = dto.IsDeleted,
                IsAlcoholic = dto.IsAlcoholic,

                //Ingredients = dto.Ingredients,

                //TODO: Mapping lists ?
            };
        }



        public CocktailDTO MapEntityToDTO(Cocktail entity)
        {
            return new CocktailDTO
            {
                Id = entity.Id,
                Name = entity.Name,
                Rating = entity.Rating,
                TimesRated = entity.TimesRated,
                ImageSrc = entity.ImageSrc,
                IsDeleted = entity.IsDeleted,
                IsAlcoholic = entity.IsAlcoholic,
                
                //Ingredients = entity.Ingredients.Select(x =>  MapEntityToDTO(x))

            };
        }
    }
}
