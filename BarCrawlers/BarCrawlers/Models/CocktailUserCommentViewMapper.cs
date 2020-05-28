using BarCrawlers.Models.Contracts;
using BarCrawlers.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarCrawlers.Models
{
    public class CocktailUserCommentViewMapper : ICocktailUserCommentViewMapper
    {
        
        public CocktailUserCommentVM MapDTOToView(CocktailUserCommentDTO dto)
        {
            var view = new CocktailUserCommentVM
            {
                CocktailId = dto.CocktailId,
                CocktailName = dto.CocktailName,
                UserId = dto.UserId,
                UserName = dto.UserName,
                Text = dto.Text,
                IsFlagged = dto.IsFlagged,
                
            };
            return view;
        }



            public CocktailUserCommentDTO MapViewToDTO(CocktailUserCommentVM view)
        {
            var dto = new CocktailUserCommentDTO
            {
                CocktailId = view.CocktailId,
                CocktailName = view.CocktailName,
                UserId = view.UserId,
                UserName = view.UserName,
                Text = view.Text,
                IsFlagged = view.IsFlagged,
            };
            return dto;
        }
    }
}
