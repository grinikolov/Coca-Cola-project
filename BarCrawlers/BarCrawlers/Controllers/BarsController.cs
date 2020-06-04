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
using Microsoft.AspNetCore.Authorization;
using BarCrawlers.Services.DTOs;

namespace BarCrawlers.Controllers
{
    public class BarsController : Controller
    {
        private readonly IBarsService _service;
        private readonly IBarViewMapper _mapper;
        private readonly ICocktailViewMapper _cocktailMapper;
        private readonly ICocktailsService _cocktailService;
        private static IEnumerable<CocktailDTO> _cocktails;
        private static IEnumerable<CocktailDTO> _cocktailsToRemove;

        public BarsController(IBarsService service, IBarViewMapper mapper, ICocktailViewMapper cocktailMapper, ICocktailsService cocktailService)
        {
            _service = service ?? throw new ArgumentNullException("Bar service not found");
            _mapper = mapper ?? throw new ArgumentNullException("Mapper not found");
            _cocktailMapper = cocktailMapper ?? throw new ArgumentNullException("Mapper not found");
            _cocktailService = cocktailService ?? throw new ArgumentNullException("Cocktail service not found");
        }

        private IEnumerable<CocktailDTO> Cocktails
        {
            get => _cocktails;
            set => _cocktails = value;
        }

        private IEnumerable<CocktailDTO> CocktailsToRemove
        {
            get => _cocktailsToRemove;
            set => _cocktailsToRemove = value;
        }

        // GET: Bars
        public async Task<IActionResult> Index(string page = "0", string itemsOnPage = "8", string searchString = null, string order = "asc")
        {
            try
            {
                var access = false;
                if (HttpContext.User.IsInRole("Magician"))
                {
                    access = true;
                }
                var bars = await this._service.GetAllAsync(page, itemsOnPage, searchString, order, access);

                ViewBag.Count = bars.Count();
                ViewBag.CurrentPage = int.Parse(page);
                ViewBag.ItemsOnPage = int.Parse(itemsOnPage);
                ViewBag.SearchString = searchString;
                ViewBag.Order = order;

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
                    var dto = _mapper.MapViewToDTO(bar);
                    dto = await _service.SetLocation(dto);
                    await _service.CreateAsync(dto);
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

            CocktailsToRemove = await _service.GetCocktailsAsync(id);
            Cocktails = await _cocktailService.GetAllAsync();
            
            
            return View(_mapper.MapDTOToView(bar));
        }

        // POST: Bars/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Rating,TimesRated,ImageSrc,IsDeleted,Phone,Email,Address,District,Town,Country,LocationId,Cocktails")] BarViewModel bar)
        {
            if (id != bar.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var dto = _mapper.MapViewToDTO(bar);
                    dto = await _service.SetLocation(dto);
                    await _service.UpdateAsync(id, dto);
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
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Rate(Guid id, Guid userId, int rating)
        {
            if (id == null || userId == null)
            {
                return NotFound();
            }

            try
            {
                await _service.RateBarAsync(id, userId, rating);
                return RedirectToAction(nameof(Details), new { id = id });
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

        [HttpPost]
        //[ValidateAntiForgeryToken]
        //[Authorize(Roles = "Magician")]
        public ActionResult AddCocktailToBar([Bind("Cocktails")] BarViewModel barVM)//[Bind("Ingredients")] 
        {
            //var ingredients = await this._ingredientsService.GetAllAsync();
            //ViewData["Cocktails"] = Cocktails.Select(c => new SelectListItem(c.Name, c.Id.ToString()));
            ViewData["Cocktails"] = Cocktails.Select(c => new SelectListItem(c.Name, c.Id.ToString()));
            ViewData["CocktailsToRemove"] = CocktailsToRemove.Select(c => new SelectListItem(c.Name, c.Id.ToString()));
            barVM.Cocktails.Add(new CocktailBarView());
            return PartialView("BarCocktails", barVM);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        //[Authorize(Roles = "Magician")]
        public ActionResult RemoveCocktailFromBar([Bind("Cocktails")] BarViewModel barVM)//[Bind("Ingredients")] 
        {
            ViewData["CocktailsToRemove"] = CocktailsToRemove.Select(c => new SelectListItem(c.Name, c.Id.ToString()));
            ViewData["Cocktails"] = Cocktails.Select(c => new SelectListItem(c.Name, c.Id.ToString()));
            barVM.Cocktails.Add(new CocktailBarView() { Remove = true});
            return PartialView("BarCocktails", barVM);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
