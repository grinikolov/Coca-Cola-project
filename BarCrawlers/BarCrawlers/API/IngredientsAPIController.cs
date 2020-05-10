using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BarCrawlers.Services.Contracts;
using BarCrawlers.Services.DTOs;

namespace BarCrawlers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngredientsAPIController : ControllerBase
    {
        private readonly IIngredientsService _service;

        public IngredientsAPIController(IIngredientsService service)
        {
            this._service = service;
        }

        // GET: api/Ingredients
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IngredientDTO>>> GetIngredients()
        {
            var ingredients = await this._service.GetAllAsync();
            return Ok(ingredients);
        }

        // GET: api/Ingredients/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IngredientDTO>> GetIngredient(Guid id)
        {
            var ingredient = await this._service.GetAsync(id);

            if (ingredient == null)
            {
                return NotFound();
            }

            return ingredient;
        }

        // PUT: api/Ingredients/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIngredient(Guid id, [FromBody] IngredientDTO ingredient)
        {
            if (ingredient == null || id != ingredient.Id)
            {
                return BadRequest();
            }
            var ingredientDTO = await this._service.UpdateAsync(id, ingredient);
            return Ok(ingredientDTO);
        }

        // POST: api/Ingredients
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<IngredientDTO>> PostIngredient([FromBody] IngredientDTO ingredient)
        {
            if (ingredient == null)
            {
                return BadRequest();
            }
            var ingredientDTO = await this._service.CreateAsync(ingredient);

            return Created("Post", ingredientDTO);
        }

        // DELETE: api/Ingredients/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteIngredient(Guid id)
        {
            return await this._service.DeleteAsync(id);
        }

    }
}
