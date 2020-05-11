﻿using BarCrawlers.Data;
using BarCrawlers.Data.DBModels;
using BarCrawlers.Services.Contracts;
using BarCrawlers.Services.DTOs;
using BarCrawlers.Services.Mappers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarCrawlers.Services
{
    public class IngredientsService : IIngredientsService
    {
        private readonly BCcontext _context;
        private readonly IngredientMapper _mapper;

        public IngredientsService(BCcontext context,
            IngredientMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        /// <summary>
        /// Gets all ingredients from the database.
        /// </summary>
        /// <returns>List of ingredients, DTOs</returns>
        public async Task<IEnumerable<IngredientDTO>> GetAllAsync()
        {
            var ingredients = await this._context.Ingredients.ToListAsync();
            
            return ingredients.Select(x => this._mapper.MapEntityToDTO(x));
        }
        /// <summary>
        /// Gets the ingredient by ID
        /// </summary>
        /// <param name="id">Ingredient ID, Guid</param>
        /// <returns>The ingredient, DTO</returns>
        public async Task<IngredientDTO> GetAsync(Guid id)
        {
            var ingredient = await this._context.Ingredients.FindAsync(id);

            if (ingredient == null)
            {
                return null;
            }

            return this._mapper.MapEntityToDTO(ingredient);
        }
        /// <summary>
        /// Creates new ingredient in the database.
        /// </summary>
        /// <param name="ingredientDTO">Ingredient DTO model.</param>
        /// <returns>The created ingredient.</returns>
        public async Task<IngredientDTO> CreateAsync(IngredientDTO ingredientDTO)
        {
            var ingredient = this._mapper.MapDTOToEntity(ingredientDTO);

            // Initialize with empty list, here not in the mapper
            ingredient.Cocktails = new List<CocktailIngredient>();

            this._context.Ingredients.Add(ingredient);
            await this._context.SaveChangesAsync();

            return this._mapper.MapEntityToDTO(ingredient);
        }
        /// <summary>
        /// Updates existing ingredient
        /// </summary>
        /// <param name="id">ID of the ingredient</param>
        /// <param name="ingredientDTO">The Ingredient DTO model with the new properties.</param>
        /// <returns>The updated ingredient, DTO</returns>
        public async Task<IngredientDTO> UpdateAsync(Guid id, IngredientDTO ingredientDTO)
        {
            var ingredient = await this._context.Ingredients.FindAsync(id);
            if (ingredient == null) return null;

            try
            {
                this._context.Entry(ingredient).State = EntityState.Modified;

                ingredient.Name = ingredientDTO.Name;
                ingredient.IsAlcoholic = ingredientDTO.IsAlcoholic;


                await this._context.SaveChangesAsync();
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
                    throw;
                }
            }

            return this._mapper.MapEntityToDTO(ingredient);
        }

        /// <summary>
        /// Deletes an ingredient from the database.
        /// </summary>
        /// <param name="id">Ingredient ID, Guid</param>
        /// <returns>True when deleted successfully, False otherwise.</returns>
        public async Task<bool> DeleteAsync(Guid id)
        {
            var ingredient = await this._context.Ingredients.FindAsync(id);
            if (ingredient == null)
            {
                return false;
            }

            this._context.Ingredients.Remove(ingredient);
            await this._context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Check if the ingredient with such ID exists in the database.
        /// </summary>
        /// <param name="id">Ingredient ID</param>
        /// <returns>True when the ingredient exists, False otherwise.</returns>
        private bool IngredientExists(Guid id)
        {
            return this._context.Ingredients.Any(e => e.Id == id);
        }
    }
}