using BarCrawlers.Services.DTOs;
using System;
using System.Threading.Tasks;

namespace BarCrawlers.Services.Contracts
{
    public interface IUserInteractionsService
    {
        //Task<BarUserCommentDTO> AddBarComment(BarUserCommentDTO commentDTO, Guid barId, Guid userId);
        Task<CocktailUserCommentDTO> AddCocktailComment(CocktailUserCommentDTO commentDTO, Guid cocktailId, Guid userId);
        //Task<BarDTO> RateBar(Guid userId, Guid barId, int theRating);
        Task<CocktailDTO> RateCocktail(int theRating, Guid cocktailId, Guid userId);
    }
}