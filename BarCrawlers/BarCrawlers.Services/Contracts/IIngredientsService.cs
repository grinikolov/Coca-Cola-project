using BarCrawlers.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BarCrawlers.Services.Contracts
{
    public interface IIngredientsService
    {
        Task<IngredientDTO> CreateAsync(IngredientDTO ingredientDTO);
        Task<bool> DeleteAsync(Guid id);
        Task<IEnumerable<IngredientDTO>> GetAllAsync();
        Task<IngredientDTO> GetAsync(Guid id);
        Task<IngredientDTO> UpdateAsync(Guid id, IngredientDTO ingredientDTO);
    }
}