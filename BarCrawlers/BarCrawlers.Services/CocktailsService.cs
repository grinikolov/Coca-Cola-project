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
        private readonly IBarMapper _barMapper;

        public CocktailsService(BCcontext context,
            ICocktailMapper mapper,
            IBarMapper barMapper)
        {
            this._context = context ?? throw new ArgumentNullException(nameof(context));
            this._mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this._barMapper = barMapper ?? throw new ArgumentNullException(nameof(barMapper));
        }

        /// <summary>
        /// Gets all cocktails from the database.
        /// </summary>
        /// <returns>List of cocktails, DTOs</returns>
        public async Task<IEnumerable<CocktailDTO>> GetAllAsync(string page, string itemsOnPage)
        {
            var cocktails = await _context.Cocktails
                .Include(c => c.Ingredients)
                    .ThenInclude(c => c.Ingredient)
                .Include(c => c.Comments)
                    .ThenInclude(c => c.User)
                .Include(c => c.Bars)
                .ToListAsync();

            return cocktails.Select(x => this._mapper.MapEntityToDTO(x)).ToList();
        }

        public async Task<IEnumerable<CocktailDTO>> GetAllAsync()
        {
            var cocktails = await _context.Cocktails
                .Include(c => c.Ingredients)
                    .ThenInclude(c => c.Ingredient)
                .Include(c => c.CocktailRatings)
                    .ThenInclude(r => r.User)
                .Include(c => c.Comments)
                    .ThenInclude(c => c.User)
                .Include(c => c.Bars)
                    .ThenInclude(b => b.Bar)
                .ToListAsync();

            return cocktails.Select(x => this._mapper.MapEntityToDTO(x)).ToList();
        }

        /// <summary>
        /// Gets page of cocktails from the database.
        /// </summary>
        /// <returns>List of cocktails, DTOs</returns>
        public async Task<IEnumerable<CocktailDTO>> GetAllAsync(string page, string itemsOnPage, string searchString, string order, bool access)
        {
            try
            {
                var p = int.Parse(page);
                var items = int.Parse(itemsOnPage);

                var cocktails = _context.Cocktails
               .Include(c => c.Ingredients)
                   .ThenInclude(c => c.Ingredient)
               .Include(c => c.CocktailRatings)
                   .ThenInclude(r => r.User)
               .Include(c => c.Comments)
                   .ThenInclude(c => c.User)
               .Include(c => c.Bars)
                   .ThenInclude(b => b.Bar)
               .AsQueryable();

                if (!access)
                {
                    cocktails = cocktails.Where(b => b.IsDeleted == false);
                }

                if (!string.IsNullOrEmpty(searchString))
                {
                    cocktails = cocktails.Where(u => u.Name.Contains(searchString));
                }

                if (order == "desc")
                {
                    cocktails = cocktails.OrderByDescending(b => b.Name);
                }
                else
                {
                    cocktails = cocktails.OrderBy(b => b.Name);
                }

                cocktails = cocktails.Skip(p * items)
                                .Take(items);

                var result = await cocktails.ToListAsync();

                return result.Select(x => this._mapper.MapEntityToDTO(x)).ToList();
            }
            catch (Exception)
            {
                throw new ArgumentException("Failed to get list");
            }
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
                .Include(c => c.CocktailRatings)
                    .ThenInclude(r => r.User)
                .Include(c => c.Comments)
                    .ThenInclude(c => c.User)
                .Include(c => c.Bars)
                    .ThenInclude(b => b.Bar)
                .FirstOrDefaultAsync(c => c.Id == id);

            //if (cocktail == null)
            //{
            //    return null;
            //}
            //if (cocktail.IsDeleted == true)
            //{
            //    throw new UnauthorizedAccessException("Not authorized to access this resource.");
            //}

            cocktail.Rating = Math.Round(cocktail.Rating, 2);

            return this._mapper.MapEntityToDTO(cocktail);
        }
        /// <summary>
        /// Creates new cocktail in the database.
        /// </summary>
        /// <param name="cocktailDTO">Cocktail DTO model.</param>
        /// <returns>The created cocktail.</returns>
        public async Task<CocktailDTO> CreateAsync(CocktailDTO cocktailDTO)
        {
            try
            {
                //TODO: Check if exists and recover
                if (await CocktailExistsByNameAsync(cocktailDTO.Name))
                {
                    var theCocktail = await this._context.Cocktails
                        .FirstOrDefaultAsync(c => c.Name.Equals(cocktailDTO.Name));
                    if (theCocktail.IsDeleted == true)
                    {
                        theCocktail.IsDeleted = false;
                    }
                    return this._mapper.MapEntityToDTO(theCocktail);

                }
                else
                {
                    var cocktail = this._mapper.MapDTOToEntity(cocktailDTO);
                    this._context.Cocktails.Add(cocktail);

                    await _context.SaveChangesAsync();

                    cocktail = await this._context.Cocktails
                    .Include(c => c.Ingredients)
                    .FirstOrDefaultAsync(c => c.Name.Equals(cocktailDTO.Name));
                    foreach (var item in cocktailDTO.Ingredients)
                    {
                        if (item.IngredientId != Guid.Empty)
                        {
                            bool isAdded = await AddIngredientsToCocktail(cocktail, item.IngredientId, item.Parts);
                            if (isAdded == false)
                            {
                                throw new OperationCanceledException("Adding cocktail ingredient failed.");
                            }
                        }
                    }

                    if (cocktail.Ingredients.Select(x => x.Ingredient).Any(i => i.IsAlcoholic))
                    {
                        cocktail.IsAlcoholic = true;
                    }
                    else
                    {
                        cocktail.IsAlcoholic = false;
                    }
                    this._context.Update(cocktail);
                    await this._context.SaveChangesAsync();

                    cocktail = await this._context.Cocktails.FirstOrDefaultAsync(x => x.Name.ToLower() == cocktailDTO.Name.ToLower());

                    return this._mapper.MapEntityToDTO(cocktail);
                }
            }
            catch (Exception e)
            {
                throw new OperationCanceledException("Fail to create cocktail");
            }
        }

        /// <summary>
        /// When creating a cocktail, it adds its CocktailIngredients to the database
        /// </summary>
        /// <param name="cocktail">The cocktail entity</param>
        /// <param name="ingredientId">Ingredient Id</param>
        /// <param name="parts">Parts of the ingredient in the cocktail</param>
        /// <returns></returns>
        public async Task<bool> AddIngredientsToCocktail(Cocktail cocktail, Guid ingredientId, int? parts)
        {
            try
            {
                var ingredient = await this._context.Ingredients
                    .FirstOrDefaultAsync(i => i.Id == ingredientId);
                var cocktailIngredient = new CocktailIngredient
                {
                    IngredientId = ingredientId,
                    Ingredient = ingredient,
                    CocktailId = cocktail.Id,
                    Cocktail = cocktail,
                    Parts = parts,
                };
                this._context.CocktailIngredients.Add(cocktailIngredient);
                await this._context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        ///// <summary>
        ///// Gets the count of all the cocktails in the database, depending on the user role.
        ///// </summary>
        ///// <param name="role"></param>
        ///// <returns>Number of all entities, when user is admin; the number of listed cocktails only for normal user</returns>
        //public async Task<int> CountAll(string role)
        //{
        //    var cocktails = this._context.Cocktails
        //        .AsQueryable();

        //    if (role == "Magician")
        //    {
        //        cocktails = cocktails.Where(x => x.IsDeleted == true);
        //    }
        //    return await cocktails.CountAsync();
        //}
        //public async Task<bool> AddIngredientsToCocktail(Guid ingredientID, Guid cocktailId)
        //{
        //    try
        //    {
        //        var cocktailIngredient = new CocktailIngredient
        //        {
        //            IngredientId = ingredientID,
        //            CocktailId = cocktailId,
        //        };
        //        this._context.CocktailIngredients.Add(cocktailIngredient);
        //        await this._context.SaveChangesAsync();
        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //}
        /// <summary>
        /// Updates existing cocktail
        /// </summary>
        /// <param name="id">ID of the cocktail</param>
        /// <param name="cocktailDTO">The cocktail with the modifications</param>
        /// <returns>Cocktail DTO.model</returns>
        public async Task<CocktailDTO> UpdateAsync(Guid id, CocktailDTO cocktailDTO)
        {
            try
            {
                if (cocktailDTO == null)
                {
                    throw new ArgumentNullException("Cocktail DTO to update is null.");
                }
                var cocktail = await this._context.Cocktails.FindAsync(id);

                this._context.Entry(cocktail).State = EntityState.Modified;

                //cocktail = this._mapper.MapDTOToEntity(cocktailDTO);

                cocktail.Id = cocktailDTO.Id;
                cocktail.Name = cocktailDTO.Name;
                cocktail.Rating = cocktailDTO.Rating;
                cocktail.TimesRated = cocktailDTO.TimesRated;
                cocktail.ImageSrc = cocktailDTO.ImageSrc;
                cocktail.IsDeleted = cocktailDTO.IsDeleted;
                cocktail.IsAlcoholic = cocktailDTO.IsAlcoholic;
                cocktail.Instructions = cocktailDTO.Instructions;

                //remove ingredients
                var previousIngredients = this._context.CocktailIngredients
                    .Where(i => i.CocktailId == cocktail.Id)
                    .AsQueryable();
                this._context.CocktailIngredients.RemoveRange(previousIngredients);

                //add new ingredients
                foreach (var item in cocktailDTO.Ingredients)
                {
                    if (item.IngredientId != Guid.Empty)
                    {
                        bool isAdded = await AddIngredientsToCocktail(cocktail, item.IngredientId, item.Parts);
                        if (isAdded == false)
                        {
                            throw new OperationCanceledException("Adding cocktail ingredient failed.");
                        }
                    }
                }

                if (cocktail.Ingredients.Select(x => x.Ingredient).Any(i => i.IsAlcoholic))
                {
                    cocktail.IsAlcoholic = true;
                }
                else
                {
                    cocktail.IsAlcoholic = false;
                }
                this._context.Update(cocktail);
                await this._context.SaveChangesAsync();

                return this._mapper.MapEntityToDTO(cocktail);
            }
            catch (Exception e)
            {
                throw new ArgumentNullException();
            }
        }

        /// <summary>
        /// Marks cocktail as IsDeleted
        /// </summary>
        /// <param name="id">ID of the cocktail</param>
        /// <returns>True when successful, false otherwise</returns>
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


        public bool CocktailExists(Guid id)
        {
            return _context.Cocktails.Any(e => e.Id == id);
        }
        public async Task<bool> CocktailExistsByNameAsync(string name)
        {
            return await _context.Cocktails.AnyAsync(e => e.Name.Equals(name));
        }

        public async Task<IEnumerable<BarDTO>> GetBarsAsync(Guid id, string page, string itemsOnPage, string searchString, bool access)
        {
            try
            {
                var bars = await this._context.CocktailBars
                    .Include(c => c.Bar)
                        .ThenInclude(b => b.Comments)
                            .ThenInclude(c => c.User)
                    .Include(c => c.Bar)
                        .ThenInclude(b => b.BarRatings)
                            .ThenInclude(r => r.User)
                    .Include(c => c.Bar)
                        .ThenInclude(b => b.Location)
                    .Where(c => c.CocktailId == id)
                    .Select(c => c.Bar)
                    .ToListAsync();

                if (!access)
                {
                    bars = bars
                        .Where(b => b.IsDeleted == false).ToList();
                };

                //var barsList = await bars.ToListAsync();

                return bars.Select(x => this._barMapper.MapEntityToDTO(x)).ToList();
            }
            catch (Exception e)
            {
                return new List<BarDTO>();
            }
        }
    }
}

