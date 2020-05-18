using BarCrawlers.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BarCrawlers.Services.Contracts
{
    public interface ICocktailsService
    {
        Task<IEnumerable<CocktailDTO>> GetAllAsync(string page, string itemsOnPage);
        Task<CocktailDTO> GetAsync(Guid id);
        Task<CocktailDTO> CreateAsync(CocktailDTO cocktailDTO);
        Task<CocktailDTO> UpdateAsync(Guid id, CocktailDTO cocktailDTO);
        Task<bool> DeleteAsync(Guid id);
    }
}