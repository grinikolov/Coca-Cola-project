using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BarCrawlers.Services.Contracts;
using System.Runtime.InteropServices;
using BarCrawlers.Services.DTOs;

namespace BarCrawlers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class CocktailsAPIController : ControllerBase
    {
        private readonly ICocktailsService _service;

        public CocktailsAPIController(ICocktailsService service)
        {
            this._service = service ?? throw new ArgumentNullException(nameof(service));
        }

        // GET: api/CocktailsAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CocktailDTO>>> GetCocktails(string page = "0", string itemsOnPage = "12")
        {
            try
            {
                var cocktails = await this._service.GetAllAsync(page, itemsOnPage);
                return Ok(cocktails);
            }
            catch (Exception)
            {
                return NoContent();
            }
        }

        // GET: api/CocktailsAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CocktailDTO>> GetCocktail(Guid id)
        {
            try
            {
                var cocktail = await this._service.GetAsync(id);
                if (cocktail == null)
                {
                    return NotFound();
                }

                return Ok(cocktail);

            }
            catch (Exception)
            {
                return NoContent();
            }

        }

        // PUT: api/CocktailsAPI/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCocktail(Guid id, CocktailDTO cocktailDTO)
        {

            try
            {
                var cocktail = await this._service.UpdateAsync(id, cocktailDTO);
                return RedirectToAction();
            }
            catch (Exception)
            {
                return NoContent();
            }
        }

        // POST: api/CocktailsAPI
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<CocktailDTO>> PostCocktail(CocktailDTO cocktailDTO)
        {
            try
            {
                var cocktail = await this._service.CreateAsync(cocktailDTO);

                return Created("PostCocktail", cocktail);

            }
            catch (Exception)
            {
                return NoContent();
            }
        }

        // DELETE: api/CocktailsAPI/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteCocktail(Guid id)
        {
            try
            {
                var result = await this._service.DeleteAsync(id);
                return Ok(result);
            }
            catch (Exception)
            {
                return NoContent();
            }
        }

    }
}
