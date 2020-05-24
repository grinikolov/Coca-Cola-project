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
            //try
        //{
            if (BarExistsByName(barDTO.Name))
            {
                var theBar = await this._context.Bars
                    .FirstOrDefaultAsync(c => c.Name.Equals(barDTO.Name));
                if (theBar.IsDeleted == true)
                {
                    theBar.IsDeleted = false;
                }
                return this._mapper.MapEntityToDTO(theBar);
            }
            else
            {
                barDTO = await SetLocation(barDTO); 
                var bar = this._mapper.MapDTOToEntity(barDTO);
                //TODO: Get the location from address

                this._context.Locations.Add(new Location { Lattitude =1,Longtitude = 1});
                await _context.SaveChangesAsync();

                var location = _context.Locations.FirstOrDefault(l => l.Longtitude == 1);

                bar.LocationId = location.Id;
                    
                this._context.Bars.Add(bar);

                await _context.SaveChangesAsync();

                bar = await this._context.Bars.FirstOrDefaultAsync(b => b.Name == barDTO.Name);

                return this._mapper.MapEntityToDTO(bar);
            }
            //}
            //catch (Exception)
            //{
            //    throw new Exception();
            //}
        }

        private bool BarExistsByName(string name)
        {
            return _context.Bars.Any(b => b.Name == name);
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
        public async Task<IEnumerable<BarDTO>> GetAllAsync(string page, string itemsOnPage, string search)
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

                if (!string.IsNullOrEmpty(search))
                {
                    bars = bars.Where(b => b.Name.Contains(search));
                }

                bars = bars.Skip(p * item)
                                .Take(item);

                var result = await bars.ToListAsync();

                var barsDTO = result.Select(b => _mapper.MapEntityToDTO(b));

                return barsDTO;
            }
            catch (Exception)
            {
                return new List<BarDTO>();
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
                return new BarDTO();
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
                //TODO: Update location from new address
                _context.Update(bar);

                await _context.SaveChangesAsync();

                return barDTO;
            }
            catch (Exception)
            {
                return new BarDTO();
            }
        }

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
                        //BarId = barId,
                        //UserId = userId,
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
                return new BarDTO();
            }

        }


        public async Task<IEnumerable<CocktailDTO>> GetCocktailsAsync(Guid id, string page, string itemsOnPage, string search)
        {
            try
            {
                var cocktails = await _context.Cocktails
                 .Include(c => c.Ingredients)
                     .ThenInclude(c => c.Ingredient)
                 .Include(c => c.Comments)
                     .ThenInclude(c => c.User)
                 .Include(c => c.Bars)
                 .Where(c => c.Id == id)
                 .ToListAsync();

                return cocktails.Select(x => this._cocktailMapper.MapEntityToDTO(x));
            }
            catch (Exception)
            {
                return new List<CocktailDTO>();
            }
        }

        private async Task<int> CountRates(Guid barId)
        {
            return await _context.BarRatings.Where(b => b.BarId == barId).CountAsync();
        }

        private async Task<double> CalculateRating(Guid id)
        {
            return await _context.BarRatings.Where(b => b.BarId == id).AverageAsync(b => b.Rating);
        }

        private async Task<BarDTO> SetLocation(BarDTO barDTO)
        {
            var street = barDTO.Address.Split();
            var httpStreet = string.Join("+", street);
            var request = new HttpRequestMessage(HttpMethod.Get,
                                                            $"search.php?q=+{httpStreet}%2C" +
                                                            $"+{barDTO.District}%2C+" +
                                                            $"{barDTO.Town}%2C+" +
                                                            $"{barDTO.Country}" +
                                                            $"&street=&city=&county=&state=&country=&postalcode=&polygon_geojson=1&viewbox=&format=json");
            //request.Headers.Add("Accept", "application/vnd.github.v3+json");
            //request.Headers.Add("User-Agent", "HttpClientFactory-Sample");

            var client = _clientFactory.CreateClient("nominatim");

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                var coordinates = await JsonSerializer.DeserializeAsync
                    <IEnumerable<object>>(responseStream);
            }
            else
            {
            }

            return barDTO;
        }
    }
}
