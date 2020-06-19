using BarCrawlers.Models;
using BarCrawlers.Models.Contracts;
using BarCrawlers.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BarCrawlers.Controllers
{
    //[Authenticate]
    public class BarUserCommentsController : Controller
    {
        //private readonly BCcontext _context;
        private readonly IBarUserCommentsService _service;
        private readonly IBarUserCommentViewMapper _mapper;

        public BarUserCommentsController(IBarUserCommentsService service, IBarUserCommentViewMapper mapper)
        {
            _service = service ?? throw new ArgumentNullException("Comment service not found");
            _mapper = mapper ?? throw new ArgumentNullException("Mapper not found");
        }

        // GET: BarUserComments
        public async Task<IActionResult> Index(Guid barId, string page = "0", string itemsOnPage = "12")
        {
            try
            {
                var comments = await _service.GetAllAsync(barId, page, itemsOnPage);

                ViewBag.Count = comments.Count();
                ViewBag.CurrentPage = int.Parse(page);
                ViewBag.ItemsOnPage = int.Parse(itemsOnPage);
                ViewBag.CurrentBar = barId;

                return View(comments.Select(b => _mapper.MapDTOToView(b)));
            }
            catch (Exception)
            {
                return Error();
            }
        }

        //// GET: BarUserComments/Details/5
        public async Task<IActionResult> Details(Guid barId, Guid userId)
        {
            if (barId == null || userId == null)
            {
                return NotFound();
            }

            var comment = await _service.GetAsync(barId, userId);

            if (comment == null)
            {
                return NotFound();
            }

            return View(_mapper.MapDTOToView(comment));
        }

        //// GET: BarUserComments/Create
        public IActionResult Create(Guid barId, Guid userId)
        {
            //ViewData["BarId"] = new SelectList(_context.Bars, "Id", "Name");
            //ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName");
            return View();
        }

        //// POST: BarUserComments/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BarId,UserId,Text,IsFlagged")] BarUserCommentView barUserComment)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    await _service.CreateAsync(_mapper.MapViewToDTO(barUserComment));
                    return RedirectToAction(nameof(Index), new { barId = barUserComment.BarId });
                }
                catch (Exception)
                {
                    return Error();
                }
                //barUserComment.BarId = Guid.NewGuid();
                //_context.Add(barUserComment);
                //await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
            }
            //ViewData["BarId"] = new SelectList(_context.Bars, "Id", "Address", barUserComment.BarId);
            //ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", barUserComment.UserId);
            return View(barUserComment);
        }

        //// GET: BarUserComments/Edit/5
        public async Task<IActionResult> Edit(Guid barId, Guid userId)
        {
            if (barId == null || userId == null)
            {
                return NotFound();
            }

            var barUserComment = await _service.GetAsync(barId, userId);
            if (barUserComment == null)
            {
                return NotFound();
            }
            //ViewData["BarId"] = new SelectList(_context.Bars, "Id", "Address", barUserComment.BarId);
            //ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", barUserComment.UserId);
            return View(_mapper.MapDTOToView(barUserComment));
        }

        //// POST: BarUserComments/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid barId, Guid userId, [Bind("BarId,UserId,Text,IsFlagged")] BarUserCommentView barUserComment)
        {
            if (barId != barUserComment.BarId || userId != barUserComment.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                //try
                //{
                var commentDTO = _mapper.MapViewToDTO(barUserComment);
                var result = await _service.UpdateAsync(commentDTO);
                //_context.Update(barUserComment);
                //await _context.SaveChangesAsync();
                //}
                //catch (DbUpdateConcurrencyException)
                //{
                //    if (!BarUserCommentExists(barUserComment.BarId))
                //    {
                //        return NotFound();
                //    }
                //    else
                //    {
                //        throw;
                //    }
                //}
                return RedirectToAction(nameof(Index), new { barId = barUserComment.BarId });
            }
            //ViewData["BarId"] = new SelectList(_context.Bars, "Id", "Address", barUserComment.BarId);
            //ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", barUserComment.UserId);
            return View(barUserComment);
        }

        //// GET: BarUserComments/Delete/5
        public async Task<IActionResult> Delete(Guid barId, Guid userId)
        {
            if (barId == null || userId == null)
            {
                return NotFound();
            }

            var barUserComment = await _service.GetAsync(barId, userId);
            if (barUserComment == null)
            {
                return NotFound();
            }

            return View(_mapper.MapDTOToView(barUserComment));
        }

        //// POST: BarUserComments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid barId, Guid userId)
        {
            if (await _service.DeleteAsync(barId, userId))
            {
                return RedirectToAction(nameof(Index), new { barId });
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
