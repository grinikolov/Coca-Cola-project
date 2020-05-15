using BarCrawlers.Data;
using BarCrawlers.Data.DBModels;
using BarCrawlers.Services.Contracts;
using BarCrawlers.Services.DTOs;
using BarCrawlers.Services.Mappers.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BarCrawlers.Services
{
    public class CocktailsService : ICocktailsService
    {
        private readonly BCcontext _context;
        private readonly ICocktailMapper _mapper;

        public CocktailsService(BCcontext context,
            ICocktailMapper mapper)
        {
            this._context = context ?? throw new ArgumentNullException(nameof(context));
            this._mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Gets all cocktails from the database.
        /// </summary>
        /// <returns>List of cocktails, DTOs</returns>
        public async Task<IEnumerable<CocktailDTO>> GetAllAsync()
        {
            var cocktails = await _context.Cocktails
                .Include(c => c.Ingredients)
                    .ThenInclude(c => c.Ingredient)
                .Include(c => c.Comment)
                    .ThenInclude(c => c.User)
                .Include(c => c.Bars)
                .ToListAsync();

            return cocktails.Select(x => this._mapper.MapEntityToDTO(x));
        }

        /// <summary>
        /// Gets the cocktail by ID
        /// </summary>
        /// <param name="id">Cocktail ID, Guid</param>
        /// <returns>The cocktail, DTO</returns>
        public async Task<CocktailDTO> GetAsync(Guid id)
        {
            var cocktail = await _context.Cocktails
                .Include(c => c.Ingredients)
                    .ThenInclude(c => c.Ingredient)
                .Include(c => c.Comment)
                    .ThenInclude(c => c.User)
                .Include(c => c.Bars)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cocktail == null)
            {
                return null;
            }
            if (cocktail.IsDeleted == true)
            {
                throw new UnauthorizedAccessException("Not authorized to access this resource.");
            }

            return this._mapper.MapEntityToDTO(cocktail);
        }
        /// <summary>
        /// Creates new cocktail in the database.
        /// </summary>
        /// <param name="cocktailDTO">Cocktail DTO model.</param>
        /// <returns>The created cocktail.</returns>
        public async Task<CocktailDTO> CreateAsync(CocktailDTO cocktailDTO)
        {
            //check if exists


            var cocktail = this._mapper.MapDTOToEntity(cocktailDTO);
            this._context.Cocktails.Add(cocktail);

            await _context.SaveChangesAsync();

            cocktail = await this._context.Cocktails.FirstOrDefaultAsync(c => c.Name == cocktailDTO.Name);
            foreach (var item in cocktailDTO.Ingredients)
            {
                var cocktailIngredient = new CocktailIngredient
                {
                    CocktailId = cocktail.Id,
                    IngredientId = item.IngredientId,
                    Parts = item.Parts,
                };
                this._context.CocktailIngredients.Add(cocktailIngredient);
            }
            await this._context.SaveChangesAsync();

            cocktail = await this._context.Cocktails.FirstOrDefaultAsync(x => x.Name.ToLower() == cocktailDTO.Name.ToLower());

            return this._mapper.MapEntityToDTO(cocktail);
        }


        public async Task<CocktailDTO> UpdateAsync(Guid id, CocktailDTO cocktailDTO)
        {
            var cocktail = await this._context.Cocktails.FindAsync(id);
            if (cocktail == null) return null;

            try
            {
                this._context.Entry(cocktail).State = EntityState.Modified;

                cocktail = this._mapper.MapDTOToEntity(cocktailDTO);

                await this._context.SaveChangesAsync();
            }
            catch (Exception)
            {
                //TODO: think what to return and when?
                if (!CocktailExists(id))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }

            return this._mapper.MapEntityToDTO(cocktail);
        }


        public async Task<bool> DeleteAsync(Guid id)
        {
            var cocktail = await this._context.Cocktails.FindAsync(id);
            if (cocktail == null)
            {
                return false;
            }
            cocktail.IsDeleted = true;

            await this._context.SaveChangesAsync();
            return true;

        }


        private bool CocktailExists(Guid id)
        {
            return _context.Cocktails.Any(e => e.Id == id);
        }

    }
}
