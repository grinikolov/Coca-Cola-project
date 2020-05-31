using BarCrawlers.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BarCrawlers.Services.Contracts
{
    public interface ICocktailCommentsService
    {
        Task<CocktailUserCommentDTO> CreateAsync(CocktailUserCommentDTO commentDTO);
        Task<bool> DeleteAsync(Guid cocktailId, Guid userId);
        Task<IEnumerable<CocktailUserCommentDTO>> GetAllAsync(Guid cocktailId, string page, string itemsOnPage);
        Task<CocktailUserCommentDTO> GetAsync(Guid cocktailId, Guid userId);
        Task<CocktailUserCommentDTO> UpdateAsync(CocktailUserCommentDTO commentDTO);
    }
}