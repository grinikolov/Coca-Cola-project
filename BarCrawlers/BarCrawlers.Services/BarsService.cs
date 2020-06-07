using BarCrawlers.Data;
using BarCrawlers.Data.DBModels;
using BarCrawlers.Services.Contracts;
using BarCrawlers.Services.DTOs;
using BarCrawlers.Services.Mappers.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using System.Diagnostics.Contracts;

namespace BarCrawlers.Services
{
    public class BarsService:IBarsService
    {
        private readonly BCcontext _context;
        private readonly IBarMapper _mapper;
        private readonly IHttpClientFactory _clientFactory;
        private readonly ICocktailMapper _cocktailMapper;

        public BarsService(BCcontext context, IBarMapper mapper, IHttpClientFactory httpClient, ICocktailMapper cocktailMapper)
        {
            this._context = context ?? throw new ArgumentNullException(nameof(context));
            this._mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this._clientFactory = httpClient ?? throw new ArgumentNullException(nameof(mapper));
            this._cocktailMapper = cocktailMapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Creates new bar in the database.
        /// </summary>
        /// <param name="barDTO">bar DTO model.</param>
        /// <returns>The created bar.</returns>
        public async Task<BarDTO> CreateAsync(BarDTO barDTO)
        {
            try
            {
                if (await BarExistsByName(barDTO.Name))
                {
                    var theBar = await this._context.Bars
                        .FirstOrDefaultAsync(c => c.Name.Equals(barDTO.Name));
                    if (theBar.IsDeleted == true)
                    {
                        theBar.IsDeleted = false;
                    }
                    _context.Bars.Update(theBar);
                    await _context.SaveChangesAsync();
                    return this._mapper.MapEntityToDTO(theBar);
                }
                else
                {
                    var bar = this._mapper.MapDTOToEntity(barDTO);
                    
                    this._context.Bars.Add(bar);

                    await _context.SaveChangesAsync();

                    bar = await this._context.Bars.FirstOrDefaultAsync(b => b.Name == barDTO.Name);

                    return this._mapper.MapEntityToDTO(bar);
                }
            }
            catch (Exception)
            {
                throw new ArgumentNullException("Fail to create bar");
            }
        }

        /// <summary>
        /// Check if bar with the name already exist.
        /// </summary>
        /// <param name="name">Name of Bar</param>
        /// <returns>Boolean</returns>
        public async Task<bool> BarExistsByName(string name)
        {
            return await _context.Bars.AnyAsync(b => b.Name == name);
        }

        /// <summary>
        /// Deletes bar record
        /// </summary>
        /// <param name="id">Bar Id, Guid</param>
        /// <returns>True on success</returns>
        public async Task<bool> DeleteAsync(Guid id)
        {
            var bar = await this._context.Bars.FindAsync(id);
            if (bar == null)
            {
                return false;
            }
            bar.IsDeleted = true;

            await this._context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Gets all bars from the database.
        /// </summary>
        /// <returns>List of bars, DTOs</returns>
        public async Task<IEnumerable<BarDTO>> GetAllAsync(string page, string itemsOnPage, string search, string order , bool access)
        {
            try
            {
                var p = int.Parse(page);
                var item = int.Parse(itemsOnPage);
                var bars = _context.Bars
                                    .Include(b => b.Cocktails)
                                    .Include(b => b.Comments)
                                    .Include(b => b.Location)
                                    .Include(b => b.BarRatings).AsQueryable();
                if (!access)
                {
                    bars = bars.Where(b => b.IsDeleted == false);
                }

                if (!string.IsNullOrEmpty(search))
                {
                    if (int.TryParse(search, out int searchNumber))
                    {
                        bars = bars.Where(b => b.Name.Contains(search)
                                || b.Address.Contains(search)
                                || b.District.Contains(search)
                                || b.Town.Contains(search) 
                                || b.Rating == searchNumber);
                    }
                    else
                    {
                        bars = bars.Where(b => b.Name.Contains(search) 
                                || b.Address.Contains(search) 
                                || b.District.Contains(search) 
                                || b.Town.Contains(search));
                    }
                    
                }

                if (order == "desc")
                {
                    bars = bars.OrderByDescending(b => b.Name);
                }
                else
                {
                    bars = bars.OrderBy(b => b.Name);
                }

                bars = bars.Skip(p * item)
                                .Take(item);

                var result = await bars.ToListAsync();

                var barsDTO = result.Select(b => _mapper.MapEntityToDTO(b));

                return barsDTO;
            }
            catch (Exception)
            {
                throw new ArgumentException("Failed to get list");
            }
        }

        /// <summary>
        /// Gets the bar by ID
        /// </summary>
        /// <param name="id">bar ID, Guid</param>
        /// <returns>The bar, DTO</returns>
        public async Task<BarDTO> GetAsync(Guid id)
        {
            try
            {
                var bar = await  _context.Bars
                                    .Include(b => b.Cocktails)
                                    .Include(b => b.Comments)
                                    .Include(b => b.Location)
                                    .Include(b => b.BarRatings)
                                    .FirstOrDefaultAsync(b => b.Id == id);

                var barDTO = _mapper.MapEntityToDTO(bar);

                return barDTO;
            }
            catch (Exception)
            {
                throw new ArgumentException();
            }
        }

        /// <summary>
        /// Update the bar
        /// </summary>
        /// <param name="id">bar ID, Guid</param>
        /// <param name="barDTO">Object of updated information, BarDTO</param>
        /// <returns>The bar, DTO</returns>
        public async Task<BarDTO> UpdateAsync(Guid id, BarDTO barDTO)
        {
            try
            {
                var bar = await _context.Bars
                                    .FirstOrDefaultAsync(b => b.Id == id);
                if (bar == null) return null;

                bar.Address = barDTO.Address;
                bar.Country = barDTO.Country;
                bar.District = barDTO.District;
                bar.Email = barDTO.Email;
                bar.ImageSrc = barDTO.ImageSrc;
                bar.Name = barDTO.Name;
                bar.Phone = barDTO.Phone;
                bar.Town = barDTO.Town;

                _context.Update(bar);

                await _context.SaveChangesAsync();

                foreach (var item in barDTO.Cocktails)
                {
                    var dbItem = await _context.CocktailBars
                        .Include(c => c.Bar)
                        .Include(c => c.Cocktail)
                        .FirstOrDefaultAsync(i => i.CocktailId == item.CocktailId && i.BarId == id);
                    if (dbItem == null)
                    {
                        var cocktail = new CocktailBar();// { CocktailId = item.CocktailId, BarId = id };
                        cocktail.Bar = await _context.Bars.FindAsync(id);
                        cocktail.BarId = cocktail.Bar.Id;
                        cocktail.Cocktail = await _context.Cocktails.FindAsync(item.CocktailId);
                        cocktail.CocktailId = cocktail.Cocktail.Id;
                        await _context.CocktailBars.AddAsync(cocktail);
                    }
                    else
                    {
                        if (item.Remove)
                        {
                            _context.CocktailBars.Remove(dbItem);
                        }
                    }
                }

                await _context.SaveChangesAsync();

                return barDTO;
            }
            catch (Exception)
            {
                throw new ArgumentNullException("Failed to update");
            }
        }

        /// <summary>
        /// Set a Rating to bar and updates properties acording to new set
        /// </summary>
        /// <param name="barId">Bar Id to be rated</param>
        /// <param name="userId">User Id making the rating</param>
        /// <param name="rating">Value of rating</param>
        /// <returns>BarDTO</returns>
        public async Task<BarDTO> RateBarAsync(Guid barId, Guid userId,  int rating)
        {
            try
            {
                var rate = await _context.BarRatings.FirstOrDefaultAsync(r => r.BarId == barId && r.UserId == userId);

                if (rate != null)
                {
                    rate.Rating = rating;

                    _context.Update(rate);
                }
                else
                {
                    var barRating = new UserBarRating
                    {
                        Rating = rating
                    };

                    barRating.Bar = await _context.Bars.FindAsync(barId);
                    barRating.User = await _context.Users.FindAsync(userId);

                    _context.BarRatings.Add(barRating);
                }

                await _context.SaveChangesAsync();

                var bar = await _context.Bars.FindAsync(barId);

                bar.Rating = await CalculateRating(barId);

                bar.TimesRated = await CountRates(barId);

                _context.Update(bar);

                await _context.SaveChangesAsync();

                return _mapper.MapEntityToDTO(bar);
            }
            catch (Exception)
            {
                throw new ArgumentException();
            }

        }

        /// <summary>
        /// Get a list of cocktails offerred in a bar with a search constraint
        /// </summary>
        /// <param name="id">Bar Id to be checked</param>
        /// <param name="page">Number of viewable page</param>
        /// <param name="itemsOnPage">Number of cocktails to be shown</param>
        /// <param name="search">Additional search constraints</param>
        /// <returns>Collection of CocktailDTO</returns>
        public async Task<IEnumerable<CocktailDTO>> GetCocktailsAsync(Guid id, string page, string itemsOnPage, string search)
        {
            try
            {
                var cocktails = await _context.CocktailBars
                    .Include(c => c.Cocktail)
                        .ThenInclude(c => c.Ingredients)
                            .ThenInclude(i => i.Ingredient)
                    .Include(c => c.Cocktail)
                        .ThenInclude(c => c.Comments)
                            .ThenInclude(c => c.User)
                    .Include(c => c.Cocktail)
                        .ThenInclude(c => c.CocktailRatings)
                            .ThenInclude(r => r.User)
                    .Include(c => c.Bar)
                        .ThenInclude(b => b.Location)
                    .Where(c => c.BarId == id)
                    .ToListAsync();

                var p = int.Parse(page);
                var item = int.Parse(itemsOnPage);

                var result = cocktails.Select(x => x.Cocktail).ToList();
                result = result.Skip(p * item).Take(item).ToList();

                return result.Select(x => this._cocktailMapper.MapEntityToDTO(x));
            }
            catch (Exception)
            {
                throw new ArgumentException("Failed to get cocktails");
            }
        }

        /// <summary>
        /// Get a list of cocktails offerred in a bar with a search constraint
        /// </summary>
        /// <param name="id">Bar Id to be checked</param>
        /// <param name="page">Number of viewable page</param>
        /// <param name="itemsOnPage">Number of cocktails to be shown</param>
        /// <param name="search">Additional search constraints</param>
        /// <returns>Collection of CocktailDTO</returns>
        public async Task<IEnumerable<CocktailDTO>> GetCocktailsAsync(Guid id)
        {
            try
            {
                var cocktails = await _context.CocktailBars
                    .Include(c => c.Cocktail)
                        .ThenInclude(c => c.Ingredients)
                            .ThenInclude(i => i.Ingredient)
                    .Include(c => c.Cocktail)
                        .ThenInclude(c => c.Comments)
                            .ThenInclude(c => c.User)
                    .Include(c => c.Cocktail)
                        .ThenInclude(c => c.CocktailRatings)
                            .ThenInclude(r => r.User)
                    .Include(c => c.Bar)
                        .ThenInclude(b => b.Location)
                    .Where(c => c.BarId == id)
                    .ToListAsync();

                return cocktails.Select(x => this._cocktailMapper.MapEntityToDTO(x.Cocktail)).ToList();
            }
            catch (Exception)
            {
                return new List<CocktailDTO>();
            }
        }

        /// <summary>
        /// Counts the made ratings for a bar
        /// </summary>
        /// <param name="barId">Bar Id to be checked</param>
        /// <returns>Int</returns>
        private async Task<int> CountRates(Guid barId)
        {
            return await _context.BarRatings.Where(b => b.BarId == barId).CountAsync();
        }

        /// <summary>
        /// Calculates the average rating of bar
        /// </summary>
        /// <param name="id">Bar Id to be checked</param>
        /// <returns>Double</returns>
        private async Task<double> CalculateRating(Guid id)
        {
            return Math.Round( await _context.BarRatings.Where(b => b.BarId == id).AverageAsync(b => b.Rating), 2);
        }

        /// <summary>
        /// Finds coordinates for given adress, creates database location object and set it to bar
        /// </summary>
        /// <param name="barDTO">The bar object to be set with location</param>
        /// <returns>BarDTO</returns>
        public async Task<BarDTO> SetLocation(BarDTO barDTO)
        {
            var street = barDTO.Address.Split();
            var httpStreet = string.Join("+", street);
            var request = new HttpRequestMessage(HttpMethod.Get,
                                                            $"search.php?q=+{httpStreet}%2C" +
                                                            $"+{barDTO.District}%2C+" +
                                                            $"{barDTO.Town}%2C+" +
                                                            $"{barDTO.Country}" +
                                                            $"&street=&city=&county=&state=&country=&postalcode=&polygon_geojson=1&viewbox=&format=json");
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("User-Agent", "BarCrawlers");

            var client = _clientFactory.CreateClient("nominatim");

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                using (var responseStream = await response.Content.ReadAsStreamAsync())
                {
                    var coordinates = await JsonSerializer.DeserializeAsync
                      <IEnumerable<LocationDTO>>(responseStream);

                    if (coordinates.Count() > 0)
                    {
                        var lat = coordinates.First().Lat;
                        var lon = coordinates.First().Lon;
                        var result = await _context.Locations.FirstOrDefaultAsync(l => l.Lattitude == lat && l.Longtitude == lon);
                        if (result == null)
                        {
                            var location = new Location { Longtitude = lon, Lattitude = lat };
                            _context.Locations.Add(location);
                            await _context.SaveChangesAsync();
                            result = await _context.Locations.FirstOrDefaultAsync(l => l.Lattitude == lat && l.Longtitude == lon);
                        }
                        var locationDTO = new LocationDTO();
                        locationDTO.Lat = result.Lattitude;
                        locationDTO.Lon = result.Longtitude;
                        barDTO.LocationId = result.Id;
                        barDTO.Location = locationDTO;
                    }
                }
            }

            return barDTO;
        }

        /// <summary>
        /// Gets Top 3 bars from the database.
        /// </summary>
        /// <returns>List of bars, DTOs</returns>
        public async Task<IEnumerable<BarDTO>> GetBestBarsAsync()
        {
            try
            {
                var bars = await _context.Bars
                                    .Include(b => b.Cocktails)
                                    .Include(b => b.Comments)
                                    .Include(b => b.Location)
                                    .Include(b => b.BarRatings)
                                    .Where(b => b.IsDeleted == false)
                                    .OrderBy(b => b.TimesRated)
                                    .ThenByDescending(b => b.Rating)
                                    .Take(3).ToListAsync();

                var barsDTO = bars.Select(b => _mapper.MapEntityToDTO(b)).ToList();

                return barsDTO;
            }
            catch (Exception)
            {
                throw new ArgumentException("Failed to get list");
            }
        }
    }
}
