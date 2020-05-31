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
            try
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
                    Instructions = dto.Instructions,
                    //Ingredients = dto.Ingredients,

                    //TODO: Mapping lists ?
                };
            }
            catch (Exception)
            {
                return new Cocktail();
            }
        }

        public CocktailDTO MapEntityToDTO(Cocktail entity)
        {
            try
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

                    Ingredients = entity.Ingredients?.Select(i => new CocktailIngredientDTO()
                    {
                        IngredientId = i.IngredientId,
                        IngredientName = i.Ingredient.Name,
                        CocktailId = i.CocktailId,
                        CocktailName = i.Cocktail.Name,
                        Parts = i.Parts
                    }).ToList(),
                    Bars = entity.Bars?.Select(b => new CocktailBarDTO
                    {
                        BarId = b.BarId,
                        BarName = b.Bar.Name,
                        CocktailId = b.CocktailId,
                        CocktailName = b.Cocktail.Name,
                    }).ToList(),
                    Comments = entity.Comments?.Select(c => new CocktailUserCommentDTO
                    {
                        CocktailId = c.CocktailId,
                        CocktailName = c.Cocktail.Name,
                        UserId = c.UserId,
                        UserName = c.User.UserName,
                        Text = c.Text,
                        IsFlagged = c.IsFlagged,
                    }).ToList(),
                    //CocktailRatings = entity.CocktailRatings.Select(r => new UserCocktailRatingDTO
                    //{
                    //    CocktailId = r.CocktailId,
                    //    CocktailName = r.Cocktail.Name,
                    //    UserId = r.UserId,
                    //    UserName = r.User.UserName,
                    //    Rating = r.Rating,

                    //}).ToList(),
                    Instructions = entity.Instructions,

                };
            }
            catch (Exception e)
            {
                return new CocktailDTO();
            }
        }
    }
}
