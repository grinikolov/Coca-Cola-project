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

namespace BarCrawlers.Services
{
    public class BarsService:IBarsService
    {
        private readonly BCcontext _context;
        private readonly IBarMapper _mapper;

        public BarsService(BCcontext context, IBarMapper mapper)
        {
            this._context = context ?? throw new ArgumentNullException(nameof(context));
            this._mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
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
    }
}
