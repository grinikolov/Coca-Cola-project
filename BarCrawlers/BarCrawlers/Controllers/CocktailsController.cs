﻿using BarCrawlers.Models;
using BarCrawlers.Models.Contracts;
using BarCrawlers.Services.Contracts;
using BarCrawlers.Services.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

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
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _ingredientsService = ingredientsService ?? throw new ArgumentNullException(nameof(ingredientsService));
            _userInteractionsService = userInteractionsService ?? throw new ArgumentNullException(nameof(userInteractionsService));
            _barMapper = barMapper ?? throw new ArgumentNullException(nameof(_barMapper));
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

                var cocktails = await _service.GetAllAsync(page, itemsOnPage, searchString, order, access);
                ViewBag.Count = cocktails.Count();
                ViewBag.CurrentPage = int.Parse(page);
                ViewBag.ItemsOnPage = int.Parse(itemsOnPage);
                ViewBag.SearchString = searchString;

                return View(cocktails.Select(c => _mapper.MapDTOToView(c)));
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

                var cocktail = await _service.GetAsync(id);

                if (cocktail == null)
                {
                    return NotFound();
                }

                //TODO: Map to ViewModel
                return View(_mapper.MapDTOToView(cocktail));

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

            Ingredients = await _ingredientsService.GetAllAsync();
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
                var cocktailDTO = _mapper.MapViewToDTO(cocktailView);
                var cocktail = await _service.CreateAsync(cocktailDTO);


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
        public ActionResult AddCocktailIngredient([Bind("Ingredients")] CocktailCreateViewModel cocktailVM)//[Bind("Ingredients")] 
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

                var cocktail = await _service.GetAsync(id);
                if (cocktail == null)
                {
                    return NotFound();
                }
                Ingredients = Ingredients ?? await _ingredientsService.GetAllAsync();
                ViewData["Ingredients"] = Ingredients.Select(x => new SelectListItem(x.Name, x.Id.ToString()));

                return View(_mapper.MapDTOToView(cocktail));
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

            var cocktail = await _service.UpdateAsync(id, _mapper.MapViewToDTO(cocktailVM));

            return RedirectToAction("Details", "Cocktails", new { id });
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

                var cocktail = await _service.GetAsync(id);
                if (cocktail == null)
                {
                    return NotFound();
                }

                return View(_mapper.MapDTOToView(cocktail));
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
            var result = await _service.DeleteAsync(id);
            if (result == true)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return await Delete(id);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Rate(Guid id, Guid userId, int rating)
        {
            if (rating <= 0 || id == default || userId == default)
            {
                return BadRequest();
            }

            try
            {
                var model = await _userInteractionsService.RateCocktail(rating, id, userId);
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
                var bars = await _service.GetBarsAsync(id, page, itemsOnPage, searchString, access);

                ViewBag.Count = bars.Count();
                ViewBag.CurrentPage = int.Parse(page);
                ViewBag.ItemsOnPage = int.Parse(itemsOnPage);
                ViewBag.SearchString = searchString;
                ViewBag.CurrentCocktail = id;

                return View(bars.Select(b => _barMapper.MapDTOToView(b)));
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Magician")]
        public async Task<IActionResult> Recover([Bind("Id")] Guid id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var cocktail = await _service.GetAsync(id);
                if (cocktail == null)
                {
                    return NotFound();
                }

                await _service.CreateAsync(cocktail);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                return RedirectToAction("Error");
            }


        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
