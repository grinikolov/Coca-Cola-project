using BarCrawlers.Data.DBModels;
using BarCrawlers.Services.DTOs;
using BarCrawlers.Services.Mappers.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace BarCrawlers.Services.Mappers
{
    public class CocktailCommentMapper : ICocktailCommentMapper
    {

        public CocktailUserComment MapDTOToEntity(CocktailUserCommentDTO dto)
        {
            try
            {
                return new CocktailUserComment
                {
                    CocktailId = dto.CocktailId,
                    //Cocktail = dto.CocktailName,
                    UserId = dto.UserId,
                    //User = dto.UserName,
                    Text = dto.Text,
                    IsFlagged = dto.IsFlagged,
                };
            }
            catch (Exception)
            {
                return new CocktailUserComment();
            }
        }
        public CocktailUserCommentDTO MapEntityToDTO(CocktailUserComment entity)
        {
            try
            {
                return new CocktailUserCommentDTO
                {
                    CocktailId = entity.CocktailId,
                    CocktailName = entity.Cocktail?.Name,
                    UserId = entity.UserId,
                    UserName = entity.Cocktail?.Name,
                    Text = entity.Text,
                    IsFlagged = entity.IsFlagged,
                };
            }
            catch (Exception)
            {
                return new CocktailUserCommentDTO();
            }
        }
    }
}