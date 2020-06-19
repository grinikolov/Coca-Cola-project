using BarCrawlers.Data;
using BarCrawlers.Data.DBModels;
using BarCrawlers.Services.Contracts;
using BarCrawlers.Services.DTOs;
using BarCrawlers.Services.Mappers.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarCrawlers.Services
{
    public class IngredientsService : IIngredientsService
    {
        private readonly BCcontext _context;
        private readonly IIngredientMapper _mapper;

        public IngredientsService(BCcontext context,
            IIngredientMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Gets all ingredients from the database.
        /// </summary>
        /// <returns>List of ingredients, DTOs</returns>
        public async Task<IEnumerable<IngredientDTO>> GetAllAsync()
        {
            var ingredients = await _context.Ingredients
                .Include(i => i.Cocktails)
                    .ThenInclude(c => c.Cocktail)
                .ToListAsync();

            return ingredients.Select(x => _mapper.MapEntityToDTO(x));
        }

        /// <summary>
        /// Gets page of ingredients from the database.
        /// </summary>
        /// <returns>List of ingredients, DTOs</returns>
        public async Task<IEnumerable<IngredientDTO>> GetAllAsync(string page, string itemsOnPage, string searchString)
        {
            var p = int.Parse(page);
            var items = int.Parse(itemsOnPage);
            var ingredients = _context.Ingredients
                .Include(i => i.Cocktails)
                    .ThenInclude(c => c.Cocktail)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                ingredients = ingredients.Where(u => u.Name.Contains(searchString));
            }

            ingredients = ingredients.Skip(p * items)
                            .Take(items);

            var result = await ingredients.ToListAsync();

            return ingredients.Select(x => _mapper.MapEntityToDTO(x));

        }

        /// <summary>
        /// Gets the ingredient by ID
        /// </summary>
        /// <param name="id">Ingredient ID, Guid</param>
        /// <returns>The ingredient, DTO</returns>
        public async Task<IngredientDTO> GetAsync(Guid id)
        {
            var ingredient = await _context.Ingredients.FindAsync(id);

            if (ingredient == null)
            {
                return null;
            }

            return _mapper.MapEntityToDTO(ingredient);
        }
        /// <summary>
        /// Creates new ingredient in the database.
        /// </summary>
        /// <param name="ingredientDTO">Ingredient DTO model.</param>
        /// <returns>The created ingredient.</returns>
        public async Task<IngredientDTO> CreateAsync(IngredientDTO ingredientDTO)
        {
            var ingredient = _mapper.MapDTOToEntity(ingredientDTO);

            // Initialize with empty list, here not in the mapper
            ingredient.Cocktails = new List<CocktailIngredient>();

            _context.Ingredients.Add(ingredient);
            await _context.SaveChangesAsync();
            ingredient = await _context.Ingredients.FirstOrDefaultAsync(x => x.Name.ToLower() == ingredientDTO.Name.ToLower());

            return _mapper.MapEntityToDTO(ingredient);
        }
        /// <summary>
        /// Updates existing ingredient
        /// </summary>
        /// <param name="id">ID of the ingredient</param>
        /// <param name="ingredientDTO">The Ingredient DTO model with the new properties.</param>
        /// <returns>The updated ingredient, DTO</returns>
        public async Task<IngredientDTO> UpdateAsync(Guid id, IngredientDTO ingredientDTO)
        {
            var ingredient = await _context.Ingredients.FindAsync(id);
            if (ingredient == null) return null;

            try
            {
                _context.Entry(ingredient).State = EntityState.Modified;

                ingredient.Name = ingredientDTO.Name;
                ingredient.IsAlcoholic = ingredientDTO.IsAlcoholic;

                _context.Update(ingredient);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                //TODO: think what to return and when?
                if (!IngredientExists(id))
                {
                    return null;
                }
                else
                {
                    throw new ArgumentException();
                }
            }

            return _mapper.MapEntityToDTO(ingredient);
        }

        /// <summary>
        /// Deletes an ingredient from the database.
        /// </summary>
        /// <param name="id">Ingredient ID, Guid</param>
        /// <returns>True when deleted successfully, False otherwise.</returns>
        public async Task<bool> DeleteAsync(Guid id)
        {
            var ingredient = await _context.Ingredients.FindAsync(id);
            if (ingredient == null)
            {
                return false;
            }

            _context.Ingredients.Remove(ingredient);
            await _context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Check if the ingredient with such ID exists in the database.
        /// </summary>
        /// <param name="id">Ingredient ID</param>
        /// <returns>True when the ingredient exists, False otherwise.</returns>
        private bool IngredientExists(Guid id)
        {
            return _context.Ingredients.Any(e => e.Id == id);
        }


        public Task<int> CountAll(string role)
        {
            throw new NotImplementedException();
        }
    }
}
