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
    public class CocktailsService : ICocktailsService
    {
        //private readonly ILogger<CocktailsService> _logger;
        private readonly BCcontext _context;
        private readonly ICocktailMapper _mapper;
        private readonly IBarMapper _barMapper;

        public CocktailsService(BCcontext context,
            ICocktailMapper mapper,
            IBarMapper barMapper
           )
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _barMapper = barMapper ?? throw new ArgumentNullException(nameof(barMapper));
            //this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
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

            return cocktails.Select(x => _mapper.MapEntityToDTO(x)).ToList();
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

            return cocktails.Select(x => _mapper.MapEntityToDTO(x)).ToList();
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

                return result.Select(x => _mapper.MapEntityToDTO(x)).ToList();
            }
            catch (Exception e)
            {
                //this._logger.LogError(e.Message);
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


            if (cocktail == null)
            {
                return null;
            }
            //if (cocktail.IsDeleted == true)
            //{
            //    throw new UnauthorizedAccessException("Not authorized to access this resource.");
            //}

            cocktail.Rating = Math.Round(cocktail.Rating, 2);

            return _mapper.MapEntityToDTO(cocktail);
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
                    var theCocktail = await _context.Cocktails
                        .FirstOrDefaultAsync(c => c.Name.Equals(cocktailDTO.Name));
                    if (theCocktail.IsDeleted == true)
                    {
                        theCocktail.IsDeleted = false;
                    }
                    _context.Cocktails.Update(theCocktail);
                    await _context.SaveChangesAsync();
                    return _mapper.MapEntityToDTO(theCocktail);

                }
                else
                {
                    var cocktail = _mapper.MapDTOToEntity(cocktailDTO);
                    _context.Cocktails.Add(cocktail);

                    await _context.SaveChangesAsync();

                    cocktail = await _context.Cocktails
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
                    _context.Update(cocktail);
                    await _context.SaveChangesAsync();

                    cocktail = await _context.Cocktails.FirstOrDefaultAsync(x => x.Name.ToLower() == cocktailDTO.Name.ToLower());

                    return _mapper.MapEntityToDTO(cocktail);
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
                var ingredient = await _context.Ingredients
                    .FirstOrDefaultAsync(i => i.Id == ingredientId);
                var cocktailIngredient = new CocktailIngredient
                {
                    IngredientId = ingredientId,
                    Ingredient = ingredient,
                    CocktailId = cocktail.Id,
                    Cocktail = cocktail,
                    Parts = parts,
                };
                _context.CocktailIngredients.Add(cocktailIngredient);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

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
                var cocktail = await _context.Cocktails.FindAsync(id);

                _context.Entry(cocktail).State = EntityState.Modified;

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
                var previousIngredients = _context.CocktailIngredients
                    .Where(i => i.CocktailId == cocktail.Id)
                    .AsQueryable();
                _context.CocktailIngredients.RemoveRange(previousIngredients);

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
                _context.Update(cocktail);
                await _context.SaveChangesAsync();

                return _mapper.MapEntityToDTO(cocktail);
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
            var cocktail = await _context.Cocktails.FindAsync(id);
            if (cocktail == null)
            {
                return false;
            }
            cocktail.IsDeleted = true;
            _context.Update(cocktail);
            await _context.SaveChangesAsync();
            return true;

        }

        /// <summary>
        /// Check if cocktail already exists
        /// </summary>
        /// <param name="id">Cocktail Id</param>
        /// <returns>True if exists, False otherwise</returns>
        public bool CocktailExists(Guid id)
        {
            return _context.Cocktails.Any(e => e.Id == id);
        }
        /// <summary>
        /// Check if cocktail with the name already exists
        /// </summary>
        /// <param name="name">Name of cocktail</param>
        /// <returns>True if exists, False otherwise</returns>
        public async Task<bool> CocktailExistsByNameAsync(string name)
        {
            return await _context.Cocktails.AnyAsync(e => e.Name.Equals(name));
        }
        /// <summary>
        /// Gets the list of bars serving the specified cocktail
        /// </summary>
        /// <param name="id">The specified cocktail Id</param>
        /// <param name="page">Page number to be loaded</param>
        /// <param name="itemsOnPage">Number of bars per page</param>
        /// <param name="searchString">Search parameter for bars</param>
        /// <param name="access">True for admin, to load unlisted bars</param>
        /// <returns></returns>
        public async Task<IEnumerable<BarDTO>> GetBarsAsync(Guid id, string page, string itemsOnPage, string searchString, bool access)
        {
            try
            {
                var bars = await _context.CocktailBars
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

                return bars.Select(x => _barMapper.MapEntityToDTO(x)).ToList();
            }
            catch (Exception e)
            {
                return new List<BarDTO>();
            }
        }
        /// <summary>
        /// Gets the three top-rated cocktails from the database
        /// </summary>
        /// <returns>The top-rated cocktails</returns>
        public async Task<IEnumerable<CocktailDTO>> GetBestCocktailsAsync()
        {
            try
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
              .Where(c => c.IsDeleted == false)
              .OrderBy(c => c.TimesRated)
              .ThenByDescending(c => c.Rating)
              .Take(3).ToListAsync();

                var cocktailsDTO = cocktails.Select(x => _mapper.MapEntityToDTO(x));

                return cocktailsDTO;
            }
            catch (Exception e)
            {
                throw new ArgumentException("Failed to get list");
            }
        }
    }
}

