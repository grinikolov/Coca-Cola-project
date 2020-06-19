using BarCrawlers.Services.DTOs;

namespace BarCrawlers.Models.Contracts
{
    public interface ICocktailUserCommentViewMapper
    {
        public CocktailUserCommentVM MapDTOToView(CocktailUserCommentDTO dto);

        public CocktailUserCommentDTO MapViewToDTO(CocktailUserCommentVM view);
    }
}
