using BarCrawlers.Models.Contracts;
using BarCrawlers.Services.DTOs;
using System;
using System.Linq;

namespace BarCrawlers.Models
{
    public class CocktailViewMapper : ICocktailViewMapper
    {
        public CocktailViewModel MapDTOToView(CocktailDTO dto)
        {
            try
            {
                return new CocktailViewModel
                {
                    Id = dto.Id,
                    Name = dto.Name,
                    Rating = dto.Rating,
                    TimesRated = dto.TimesRated,
                    ImageSrc = dto.ImageSrc,
                    IsDeleted = dto.IsDeleted,
                    IsAlcoholic = dto.IsAlcoholic,
                    Instructions = dto.Instructions,
                    Ingredients = dto.Ingredients.Select(i => new CocktailIngredientViewModel()
                    {
                        IngredientId = i.IngredientId,
                        IngredientName = i.IngredientName,
                        Parts = i.Parts
                    }).ToList(),
                    //Comments = dto.Comments,
                    //TODO: Mapping lists ?
                    Bars = dto.Bars.Select(b => new CocktailBarView
                    {
                        BarId = b.BarId,
                        BarName = b.BarName,
                        CocktailId = b.CocktailId,
                        CocktailName = b.CocktailName,
                    }).ToList(),
                    Comments = dto.Comments.Select(c => new CocktailUserCommentVM
                    {
                        CocktailId = c.CocktailId,
                        CocktailName = c.CocktailName,
                        UserId = c.UserId,
                        UserName = c.UserName,
                        Text = c.Text,
                        IsFlagged = c.IsFlagged,
                    }).ToList(),
                    CocktailRatings = dto.CocktailRatings.Select(r => new UserCocktailRatingDTO
                    {
                        CocktailId = r.CocktailId,
                        CocktailName = r.CocktailName,
                        UserId = r.UserId,
                        UserName = r.UserName,
                        Rating = r.Rating,

                    }).ToList(),
                    //Instructions = dto.Instructions,
                };
            }
            catch (Exception)
            {
                return new CocktailViewModel();
            }
        }

        public CocktailDTO MapViewToDTO(CocktailViewModel view)
        {
            try
            {
                return new CocktailDTO
                {
                    Id = view.Id,
                    Name = view.Name,
                    Rating = view.Rating,
                    TimesRated = view.TimesRated,
                    ImageSrc = view.ImageSrc,
                    IsDeleted = view.IsDeleted,
                    IsAlcoholic = view.IsAlcoholic,

                    Ingredients = view.Ingredients?.Select(i => new CocktailIngredientDTO()
                    {
                        IngredientId = i.IngredientId,
                        IngredientName = i.IngredientName,
                        //CocktailId = i.CocktailId,
                        //CocktailName = i.CocktailName,
                        Parts = i.Parts
                    }).ToList(),
                    Bars = view.Bars?.Select(b => new CocktailBarDTO
                    {
                        BarId = b.BarId,
                        BarName = b.BarName,
                        CocktailId = b.CocktailId,
                        CocktailName = b.CocktailName,
                    }).ToList(),
                    Comments = view.Comments?.Select(c => new CocktailUserCommentDTO
                    {
                        CocktailId = c.CocktailId,
                        CocktailName = c.CocktailName,
                        UserId = c.UserId,
                        UserName = c.UserName,
                        Text = c.Text,
                        IsFlagged = c.IsFlagged,
                    }).ToList(),
                    CocktailRatings = view.CocktailRatings?.Select(r => new UserCocktailRatingDTO
                    {
                        CocktailId = r.CocktailId,
                        CocktailName = r.CocktailName,
                        UserId = r.UserId,
                        UserName = r.UserName,
                        Rating = r.Rating,

                    }).ToList(),
                    Instructions = view.Instructions,

                };
            }
            catch (Exception)
            {
                return new CocktailDTO();
            }
        }

        public CocktailDTO MapViewToDTO(CocktailCreateViewModel view)
        {
            try
            {
                return new CocktailDTO
                {
                    Id = view.Id,
                    Name = view.Name,
                    Rating = view.Rating,
                    TimesRated = view.TimesRated,
                    ImageSrc = view.ImageSrc,
                    IsDeleted = view.IsDeleted,
                    IsAlcoholic = view.IsAlcoholic,

                    Instructions = view.Instructions,
                    Ingredients = view.Ingredients.Select(i => new CocktailIngredientDTO()
                    {
                        IngredientId = i.IngredientId,
                        Parts = i.Parts
                    }).ToList(),
                };


            }
            catch (Exception)
            {
                return new CocktailDTO();
            }
        }
    }
}