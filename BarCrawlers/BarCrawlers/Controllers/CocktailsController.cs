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
        private readonly IBarViewMapper _barMapper;
        private readonly IIngredientsService _ingredientsService;
        private readonly IUserInteractionsService _userInteractionsService;
        private static IEnumerable<IngredientDTO> _ingredients;
        public CocktailsController(ICocktailsService service,
             ICocktailViewMapper mapper,
             IIngredientsService ingredientsService,
             IUserInteractionsService userInteractionsService,
             IBarViewMapper barMapper)
        {
            this._service = service ?? throw new ArgumentNullException(nameof(service));
            this._mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this._ingredientsService = ingredientsService ?? throw new ArgumentNullException(nameof(ingredientsService));
            this._userInteractionsService = userInteractionsService ?? throw new ArgumentNullException(nameof(userInteractionsService));
            this._barMapper = barMapper ?? throw new ArgumentNullException(nameof(_barMapper));
        }
        public IEnumerable<IngredientDTO> Ingredients
        {
            get => _ingredients;
            private set => _ingredients = value;
        }


        // GET: Cocktails
        public async Task<IActionResult> Index(string page = "0", string itemsOnPage = "12", string searchString = null, string order = "asc")
        {
            try
            {
                var access = false;
                if (HttpContext.User.IsInRole("Magician"))
                {
                    access = true;
                }

                var cocktails = await this._service.GetAllAsync(page, itemsOnPage, searchString, order, access);
                ViewBag.Count = cocktails.Count();
                ViewBag.CurrentPage = int.Parse(page);
                ViewBag.ItemsOnPage = int.Parse(itemsOnPage);
                ViewBag.SearchString = searchString;

                return View(cocktails.Select(c => this._mapper.MapDTOToView(c)));
            }
            catch (Exception)
            {
                return RedirectToAction("Error");
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

                //TODO: Map to ViewModel
                return View(this._mapper.MapDTOToView(cocktail));

            }
            catch (Exception)
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Cocktails/Create
        [Authorize(Roles = "Magician")]
        public async Task<IActionResult> Create()
        {
            //ViewData["Ingredient"] = new SelectList( await this._ingredientsService.GetAllAsync(), "ID", "Name");

            Ingredients = await this._ingredientsService.GetAllAsync();
            ViewData["Ingredients"] = Ingredients.Select(x => new SelectListItem(x.Name, x.Id.ToString()));

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
                //TODO: Initialize new List<CocktailIngredientViewModel> Ingredients  maybe?

                //if (ModelState.IsValid)
                //{
                var cocktailDTO = this._mapper.MapViewToDTO(cocktailView);
                var cocktail = await this._service.CreateAsync(cocktailDTO);


                return RedirectToAction("Index");
                //}
                // return await Create();
            }
            catch (Exception)
            {
                return RedirectToAction("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Magician")]
        public async Task<ActionResult> AddCocktailIngredient([Bind("Ingredients")] CocktailCreateViewModel cocktailVM)//[Bind("Ingredients")] 
        {
            //var ingredients = await this._ingredientsService.GetAllAsync();
            ViewData["Ingredients"] = Ingredients.Select(x => new SelectListItem(x.Name, x.Id.ToString()));

            cocktailVM.Ingredients.Add(new CocktailIngredientViewModel());
            return PartialView("CocktailIngredients", cocktailVM);
        }

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
                Ingredients= Ingredients ?? await this._ingredientsService.GetAllAsync();
                ViewData["Ingredients"] = Ingredients.Select(x => new SelectListItem(x.Name, x.Id.ToString()));

                return View(this._mapper.MapDTOToView(cocktail));
            }
            catch (Exception)
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Cocktails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Magician")]
        public async Task<IActionResult> Edit(Guid id, CocktailViewModel cocktailVM)
        {
            if (id != cocktailVM.Id)
            {
                return NotFound();
            }

            var cocktail = await this._service.UpdateAsync(id, this._mapper.MapViewToDTO(cocktailVM));

            return View(this._mapper.MapDTOToView(cocktail));
        }

        // GET: Cocktails/Delete/5
        [Authorize(Roles = "Magician")]
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

                return View(this._mapper.MapDTOToView(cocktail));
            }
            catch (Exception)
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Cocktails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Magician")]
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

        public async Task<IActionResult> Rate( Guid id, Guid userId, int rating)
        {
            if (rating <= 0 || id == default || userId == default)
            {
                return BadRequest();
            }

            try
            {
                var model = await this._userInteractionsService.RateCocktail(rating, id, userId);
                return RedirectToAction("Details", "Cocktails", new { id = id });
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }
        public async Task<IActionResult> LoadBars(Guid id, string page = "0", string itemsOnPage = "12", string searchString = null)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var access = false;
                if (HttpContext.User.IsInRole("Magician"))
                {
                    access = true;
                }
                var bars = await this._service.GetBarsAsync(id, page, itemsOnPage, searchString, access);

                ViewBag.Count = bars.Count();
                ViewBag.CurrentPage = int.Parse(page);
                ViewBag.ItemsOnPage = int.Parse(itemsOnPage);
                ViewBag.SearchString = searchString;
                ViewBag.CurrentCocktail = id;

                return View(bars.Select(b => this._barMapper.MapDTOToView(b)));
            }
            catch (Exception)
            {
                return RedirectToAction("Error");
            }

            //var result = await _service.GetCocktails();

            //return View();
        }
        //public async Task<IActionResult> Comment(Guid userId, Guid cocktailId, CocktailUserCommentVM commentVm)
        //{
        //    try
        //    {
        //        var commentDTO = this._cocktailUserCommentViewMapper(commentVM);
        //        var comment = await this._userInteractionsService.AddCocktailComment(commentDTO, cocktailId, userId);
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
