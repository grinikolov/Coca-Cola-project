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
using Microsoft.AspNetCore.Authorization;
using BarCrawlers.Services.DTOs;
using BarCrawlers.Models;
using System.Diagnostics;

namespace BarCrawlers.Areas.Magician.Controllers
{
    [Area("Magician")]
    public class IngredientsController : Controller
    {
        private readonly IIngredientsService _service;

        public IngredientsController(IIngredientsService service)
        {
            this._service = service ?? throw new ArgumentNullException(nameof(service));
        }

        // GET: Magician/Ingredients
        public async Task<IActionResult> Index(string page = "0", string itemsOnPage = "12", string searchString = null)
        {
            var ingredients = await this._service.GetAllAsync(page, itemsOnPage, searchString);
            ViewBag.Count = ingredients.Count();
            ViewBag.CurrentPage = int.Parse(page);
            ViewBag.ItemsOnPage = int.Parse(itemsOnPage);
            ViewBag.SearchString = searchString; 
            return View(ingredients);
        }

        // GET: Magician/Ingredients/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            var ingredient = await this._service.GetAsync(id);
            return View(ingredient);
        }

        // GET: Magician/Ingredients/Create
        [HttpGet]
        [Authorize(Roles = "Magician")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Magician/Ingredients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Magician")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IngredientDTO ingredientDTO)
        {
            //if (ModelState.IsValid)
            //{
            //    return View(ingredientDTO);
            //}
            await this._service.CreateAsync(ingredientDTO);
            return RedirectToAction("Index", "Ingredients");
        }

        // GET: Magician/Ingredients/Edit/5        
        [Authorize(Roles = "Magician")]
        public async Task<IActionResult> Edit(Guid id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }
                var ingredient = await this._service.GetAsync(id);
                if (ingredient == null)
                {
                    return NotFound();
                }
                return View(ingredient);
            }
            catch (Exception)
            {
                return Error();
            }
        }

        // POST: Magician/Ingredients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Magician")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, IngredientDTO ingredientDTO)
        {
            if (id != ingredientDTO.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var ingredient = await this._service.UpdateAsync(id, ingredientDTO);

                }
                catch (Exception)
                {
                    return Error();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(ingredientDTO);
        }

        // GET: Magician/Ingredients/Delete/5       
        [Authorize(Roles = "Magician")]

        public async Task<IActionResult> Delete(Guid id)
        {
            var ingredient = await this._service.GetAsync(id);

            return View(ingredient);
        }

        // POST: Magician/Ingredients/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Magician")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id, IngredientDTO ingredientDTO)
        {
            try
            {
                var isDeleted = await this._service.DeleteAsync(id);
                if (isDeleted)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return View();
                }
            }
            catch (Exception)
            {
                return Error();
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
