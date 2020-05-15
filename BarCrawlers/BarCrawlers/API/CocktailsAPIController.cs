using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BarCrawlers.Data;
using BarCrawlers.Data.DBModels;

namespace BarCrawlers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class CocktailsAPIController : ControllerBase
    {
        private readonly BCcontext _context;

        public CocktailsAPIController(BCcontext context)
        {
            _context = context;
        }

        // GET: api/CocktailsAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cocktail>>> GetCocktails()
        {
            return await _context.Cocktails.ToListAsync();
        }

        // GET: api/CocktailsAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cocktail>> GetCocktail(Guid id)
        {
            var cocktail = await _context.Cocktails.FindAsync(id);

            if (cocktail == null)
            {
                return NotFound();
            }

            return cocktail;
        }

        // PUT: api/CocktailsAPI/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCocktail(Guid id, Cocktail cocktail)
        {
            if (id != cocktail.Id)
            {
                return BadRequest();
            }

            _context.Entry(cocktail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CocktailExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/CocktailsAPI
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Cocktail>> PostCocktail(Cocktail cocktail)
        {
            _context.Cocktails.Add(cocktail);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCocktail", new { id = cocktail.Id }, cocktail);
        }

        // DELETE: api/CocktailsAPI/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Cocktail>> DeleteCocktail(Guid id)
        {
            var cocktail = await _context.Cocktails.FindAsync(id);
            if (cocktail == null)
            {
                return NotFound();
            }

            _context.Cocktails.Remove(cocktail);
            await _context.SaveChangesAsync();

            return cocktail;
        }

        private bool CocktailExists(Guid id)
        {
            return _context.Cocktails.Any(e => e.Id == id);
        }
    }
}
