using BarCrawlers.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarCrawlers.Models.Contracts
{
    public interface ICocktailUserCommentViewMapper
    {
        public CocktailUserCommentVM MapDTOToView(CocktailUserCommentDTO dto);

        public CocktailUserCommentDTO MapViewToDTO(CocktailUserCommentVM view);
    }
}
