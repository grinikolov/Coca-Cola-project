using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BarCrawlers.Data;
using BarCrawlers.Data.DBModels;
using BarCrawlers.Services.Contracts;
using BarCrawlers.Models.Contracts;
using System.Diagnostics;
using BarCrawlers.Models;

namespace BarCrawlers.Controllers
{
    public class BarsController : Controller
    {
        private readonly IBarsService _service;
        private readonly IBarViewMapper _mapper;
        private readonly ICocktailViewMapper _cocktailMapper;

        public BarsController(IBarsService service, IBarViewMapper mapper, ICocktailViewMapper cocktailMapper)
        {
            _service = service ?? throw new ArgumentNullException("Bar service not found");
            _mapper = mapper ?? throw new ArgumentNullException("Mapper not found");
            _cocktailMapper = cocktailMapper ?? throw new ArgumentNullException("Mapper not found");
        }

        // GET: Bars
        public async Task<IActionResult> Index(string page = "0", string itemsOnPage = "12", string searchString = null)
        {
            try
            {
                var bars = await this._service.GetAllAsync(page, itemsOnPage, searchString);

                ViewBag.Count = bars.Count();
                ViewBag.CurrentPage = int.Parse(page);
                ViewBag.ItemsOnPage = int.Parse(itemsOnPage);
                ViewBag.SearchString = searchString;

                return View(bars.Select(b => this._mapper.MapDTOToView(b)));
            }
            catch (Exception)
            {
                return Error();
            }
        }

        // GET: Bars/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var bar = await this._service.GetAsync(id);

                if (bar == null)
                {
                    return NotFound();
                }

                return View(_mapper.MapDTOToView(bar));

            }
            catch (Exception)
            {
                return Error();
            }
        }

        // GET: Bars/Create
        public IActionResult Create()
        {
            //ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Id");
            return View();
        }

        // POST: Bars/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Rating,TimesRated,ImageSrc,Phone,Email,Address,District,Town,Country")] BarViewModel bar)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _service.CreateAsync(_mapper.MapViewToDTO(bar));
                    return RedirectToAction(nameof(Index));
                }
                    catch (Exception)
                {
                    return Error();
                }
            }
            //ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Id", bar.LocationId);
            return View(bar);
        }

        // GET: Bars/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bar = await _service.GetAsync(id);
            if (bar == null)
            {
                return NotFound();
            }
            //ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Id", bar.LocationId);
            return View(_mapper.MapDTOToView(bar));
        }

        // POST: Bars/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Rating,TimesRated,ImageSrc,IsDeleted,Phone,Email,Address,District,Town,Country,LocationId")] BarViewModel bar)
        {
            if (id != bar.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _service.UpdateAsync(id, _mapper.MapViewToDTO(bar));
                }
                catch (Exception)
                {
                    return Error();
                }
                return RedirectToAction(nameof(Index));
            }
            //ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Id", bar.LocationId);
            return View(bar);
        }

        // GET: Bars/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bar = await _service.GetAsync(id);
            if (bar == null)
            {
                return NotFound();
            }

            return View(_mapper.MapDTOToView(bar));
        }

        // POST: Bars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var result = await _service.DeleteAsync(id);
            if (result)
            {
                return RedirectToAction(nameof(Index));
            }
            return Error();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Rate(Guid barId, Guid userId, [Bind("Rating")] int rating)
        {
            if (barId == null || userId == null)
            {
                return NotFound();
            }

            try
            {
                await _service.RateBarAsync(barId, userId, rating);
                return RedirectToAction(nameof(Details), new { id = barId });
        }
            catch (Exception)
            {
                return Error();
            }
        }

        public async Task<IActionResult> LoadCocktails(Guid id, string page = "0", string itemsOnPage = "12", string searchString = null)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var bars = await this._service.GetCocktailsAsync(id, page, itemsOnPage, searchString);

                ViewBag.Count = bars.Count();
                ViewBag.CurrentPage = int.Parse(page);
                ViewBag.ItemsOnPage = int.Parse(itemsOnPage);
                ViewBag.SearchString = searchString;
                ViewBag.CurrentBar = id;

                return View(bars.Select(b => this._cocktailMapper.MapDTOToView(b)));
            }
            catch (Exception)
            {
                return Error();
            }

            //var result = await _service.GetCocktails();

            //return View();
        }

        //private bool BarExists(Guid id)
        //{
        //    return _context.Bars.Any(e => e.Id == id);
        //}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
