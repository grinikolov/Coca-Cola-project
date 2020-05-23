using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BarCrawlers.Data;
using BarCrawlers.Services.Contracts;
using BarCrawlers.Models.Contracts;
using BarCrawlers.Models;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Internal;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using BarCrawlers.Services.DTOs;

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
        public async Task<IActionResult> Index(string page = "0", string itemsOnPage = "12", string searchString = null)
        {
            try
            {
                ////TODO: searchString cocktails
                //var role = this.User.FindFirstValue(ClaimTypes.Role);
                //var count = await this._service.CountAll(role);

                var cocktails = await this._service.GetAllAsync(page, itemsOnPage, searchString);
                ViewBag.Count = cocktails.Count();
                ViewBag.CurrentPage = int.Parse(page);
                ViewBag.ItemsOnPage = int.Parse(itemsOnPage);
                ViewBag.SearchString = searchString;

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
        public async Task<IActionResult> Create()
        {
            ViewData["Ingredient"] = new SelectList(this._ingredientsService.GetAllAsync().Result, "ID", "Name");
            var ingredients = await this._ingredientsService.GetAllAsync();
            ViewData["Ingredients"] = ingredients.Select(x => new SelectListItem(x.Name, x.Id.ToString()));

            return View();
        }

        // POST: Cocktails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Magician")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CocktailCreateViewModel cocktailView)
        {
            try
            {
                //if (ModelState.IsValid)
                //{
                    var cocktailDTO = this._mapper.MapViewToDTO(cocktailView);
                    var cocktail = await this._service.CreateAsync(cocktailDTO);

                    foreach (var item in cocktailView.Ingredients)
                    {
                        //TODO: Parts of ingredient in Cocktail:
                        if (!await this._service.AddIngredientsToCocktail(item.IngredientId, cocktail.Id,item.Parts)) //item.IngredientId, cocktail.Id, item.Parts
                        {
                            return Error();
                        }
                    }

                    return RedirectToAction(nameof(Index));
                //}
                return await Create();
            }
            catch (Exception)
            {
                return Error();
            }
        }
        // TODO: Edit cocktail!

        // GET: Cocktails/Edit/5

        [Authorize(Roles = "Magician")]
        public async Task<IActionResult> Edit(Guid id)
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

        // POST: Cocktails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, CocktailDTO cocktailDTO)
        {
            if (id != cocktailDTO.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var cocktail = await this._service.UpdateAsync(id, cocktailDTO);
                }
                catch (Exception)
                {
                    return Error();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(cocktailDTO);
        }

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
