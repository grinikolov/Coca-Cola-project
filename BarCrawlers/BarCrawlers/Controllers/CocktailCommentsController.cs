using BarCrawlers.Models;
using BarCrawlers.Models.Contracts;
using BarCrawlers.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BarCrawlers.Controllers
{
    public class CocktailCommentsController : Controller
    {
        private readonly ICocktailCommentsService _service;
        private readonly ICocktailUserCommentViewMapper _mapper;

        public CocktailCommentsController(ICocktailCommentsService service,
            ICocktailUserCommentViewMapper mapper)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        // GET: CocktailComments
        public async Task<IActionResult> Index(Guid cocktailId, string page = "0", string itemsOnPage = "12")
        {
            try
            {
                var comments = await _service.GetAllAsync(cocktailId, page, itemsOnPage);

                ViewBag.Count = comments.Count();
                ViewBag.CurrentPage = int.Parse(page);
                ViewBag.ItemsOnPage = int.Parse(itemsOnPage);
                ViewBag.CurrentCocktail = cocktailId;

                return View(comments.Select(b => _mapper.MapDTOToView(b)));
            }
            catch (Exception)
            {
                return Error();
            }
        }

        //// GET: CocktailComments/Details/5
        public async Task<IActionResult> Details(Guid cocktailId, Guid userId)
        {
            if (cocktailId == null || userId == null)
            {
                return NotFound();
            }

            var comment = await _service.GetAsync(cocktailId, userId);

            if (comment == null)
            {
                return NotFound();
            }

            return View(_mapper.MapDTOToView(comment));
        }

        //// GET: CocktailComments/Create
        //[Authorize]
        public IActionResult Create(Guid cocktailId, Guid userId)
        {
            ViewData["CocktailId"] = cocktailId;
            ViewData["UserId"] = userId;

            return View();
        }

        //// POST: CocktailComments/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize]
        public async Task<IActionResult> Create(CocktailUserCommentVM cocktailUserCommentVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var commentDTO = _mapper.MapViewToDTO(cocktailUserCommentVM);
                    await _service.CreateAsync(commentDTO);
                    return RedirectToAction(nameof(Index), new { cocktailId = cocktailUserCommentVM.CocktailId });
                }
                catch (Exception)
                {
                    return Error();
                }
            }
            return View(cocktailUserCommentVM);
        }

        //// GET: CocktailComments/Edit/5
        //[Authorize]
        public async Task<IActionResult> Edit(Guid cocktailId, Guid userId)
        {
            if (cocktailId == null || userId == null)
            {
                return NotFound();
            }

            var cocktailUserComment = await _service.GetAsync(cocktailId, userId);
            if (cocktailUserComment == null)
            {
                return NotFound();
            }
            return View(_mapper.MapDTOToView(cocktailUserComment));
        }

        //// POST: CocktailComments/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize]
        public async Task<IActionResult> Edit(Guid cocktailId, Guid userId, [Bind("CocktailId,UserId,Text,IsFlagged")] CocktailUserCommentVM cocktailUserComment)
        {
            if (cocktailId != cocktailUserComment.CocktailId || userId != cocktailUserComment.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var commentDTO = _mapper.MapViewToDTO(cocktailUserComment);
                    var result = await _service.UpdateAsync(commentDTO);
                }
                catch (DbUpdateConcurrencyException)
                {
                    return Error();
                }
                return RedirectToAction(nameof(Index), new { cocktailId = cocktailUserComment.CocktailId });
            }
            return View(cocktailUserComment);
        }

        //// GET: CocktailComments/Delete/5
        public async Task<IActionResult> Delete(Guid cocktailId, Guid userId)
        {
            if (cocktailId == null || userId == null)
            {
                return NotFound();
            }

            var cocktailUserComment = await _service.GetAsync(cocktailId, userId);
            if (cocktailUserComment == null)
            {
                return NotFound();
            }

            return View(_mapper.MapDTOToView(cocktailUserComment));
        }

        //// POST: CocktailComments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        //[Authorize]
        public async Task<IActionResult> DeleteConfirmed(Guid cocktailId, Guid userId)
        {
            if (await _service.DeleteAsync(cocktailId, userId))
            {
                return RedirectToAction(nameof(Index), new { cocktailId });
            }
            else
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
