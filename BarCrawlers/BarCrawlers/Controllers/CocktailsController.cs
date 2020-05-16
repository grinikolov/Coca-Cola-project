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
using BarCrawlers.Models;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Internal;
using System.Diagnostics;

namespace BarCrawlers.Controllers
{
    public class CocktailsController : Controller
    {
        private readonly ICocktailsService _service;
        private readonly ICocktailViewMapper _mapper;
        private readonly IIngredientsService _ingredientsService;

        public CocktailsController(ICocktailsService service,
            ICocktailViewMapper mapper,
             IIngredientsService ingredientsService)
        {
            this._service = service ?? throw new ArgumentNullException(nameof(service));
            this._mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this._ingredientsService = ingredientsService ?? throw new ArgumentNullException(nameof(ingredientsService));
        }

        // GET: Cocktails
        public async Task<IActionResult> Index(string page = "0", string itemsOnPage = "12")
        {
            try
            {
                var cocktails = await this._service.GetAllAsync(page, itemsOnPage);

                return View(cocktails.Select(c => this._mapper.MapDTOToView(c)));
            }
            catch (Exception)
            {
                return Error();
            }
        }

        // GET: Cocktails/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var cocktail = await this._service.GetAsync(id);

                if (cocktail == null)
                {
                    return NotFound();
                }

                return View(cocktail);

            }
            catch (Exception)
            {
                return Error();
            }
        }

        // GET: Cocktails/Create
        public IActionResult Create()
        {
            ViewData["IngredientId"] = new SelectList(this._ingredientsService.GetAllAsync().Result, "ID", "Name");

            return View();
        }

        // POST: Cocktails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Rating,TimesRated,ImageSrc,IsDeleted,IsAlcoholic,Instructions")] CocktailViewModel cocktailView)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    var cocktailDTO = this._mapper.MapViewToDTO(cocktailView);
                    var cocktail = await this._service.CreateAsync(cocktailDTO);
                    return RedirectToAction(nameof(Index));
                }
                return Create();
            }
            catch (Exception)
            {
                return Error();
            }
        }
        /* TODO: Edit cocktail!
         
        // GET: Cocktails/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var cocktail = await _context.Cocktails.FindAsync(id);
                if (cocktail == null)
                {
                    return NotFound();
                }
                return View(cocktail);
            }
            catch (Exception)
            {
                return Error();
            }
        }

        // POST: Cocktails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Rating,TimesRated,ImageSrc,IsDeleted,IsAlcoholic,Instructions")] Cocktail cocktail)
        {
            if (id != cocktail.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cocktail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CocktailExists(cocktail.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(cocktail);
        }*/

        // GET: Cocktails/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var cocktail = await this._service.GetAsync(id);
                if (cocktail == null)
                {
                    return NotFound();
                }

                return View(cocktail);
            }
            catch (Exception)
            {
                return Error();
            }
        }

        // POST: Cocktails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var result = await this._service.DeleteAsync(id);
            if (result == true)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return await Delete(id);
            }
        }

        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
