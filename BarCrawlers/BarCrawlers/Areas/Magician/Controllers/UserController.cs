using BarCrawlers.Areas.Magician.Models.Contrtacts;
using BarCrawlers.Data.DBModels;
using BarCrawlers.Services.Contracts;
using BarCrawlers.Services.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BarCrawlers.Areas.Magician.Controllers
{
    [Area("Magician")]
    [Authorize(Roles = "Magician")]
    public class UserController : Controller
    {
        private readonly IUsersService _service;
        private readonly IUserViewMapper _mapper;
        private readonly UserManager<User> _userManager;

        public UserController(IUsersService service, IUserViewMapper mapper, UserManager<User> userManager)
        {
            _service = service ?? throw new ArgumentNullException("User service not found");
            _mapper = mapper ?? throw new ArgumentNullException("User mapper not found");
            _userManager = userManager ?? throw new ArgumentNullException("User manager not found");
        }

        // GET: Magician/User
        public async Task<IActionResult> Index(string page = "0", string itemsOnPage = "12", string searchString = null)
        {
            try
            {
                var serviceResult = await _service.GetAllAsync(page, itemsOnPage, searchString);
                ViewBag.Count = serviceResult.Count();
                ViewBag.CurrentPage = int.Parse(page);
                ViewBag.ItemsOnPage = int.Parse(itemsOnPage);
                ViewBag.SearchString = searchString;
                return View(serviceResult.Select(u => _mapper.MapDTOToView(u)));
            }
            catch (Exception)
            {
                return View();
            }

        }

        // GET: Magician/User/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }
            try
            {
                var user = _mapper.MapDTOToView(await _service.GetAsync(id));
                if (user == null)
                {
                    return NotFound();
                }
                return View(user);
            }
            catch (Exception)
            {
                return View();
            }
        }

        // GET: Magician/User/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Magician/User/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("ImageSrc,Id,UserName,Email,EmailConfirmed,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount")] User user)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        user.Id = Guid.NewGuid();
        //        _context.Add(user);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(user);
        //}

        // GET: Magician/User/Edit/5
        //public async Task<IActionResult> Edit(Guid id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var user = _mapper.MapDTOToView(await _service.GetAsync(id));
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(user);
        //}

        // POST: Magician/User/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Id")] Guid id)
        {
            //if (id != user.Id)
            //{
            //    return NotFound();
            //}

            if (ModelState.IsValid)
            {
                try
                {
                    var userDTO = new UserDTO() { Id = id, LockoutEnd = DateTime.UtcNow.AddDays(7) };
                    await _service.UpdateAsync(id, userDTO, _userManager);
                }
                catch (Exception)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unban(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _service.UnbanAsync(id, _userManager);
            if (user == null)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Magician/User/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(Guid id)
        //{
        //    var user = await _context.Users.FindAsync(id);
        //    _context.Users.Remove(user);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool UserExists(Guid id)
        //{
        //    return _context.Users.Any(e => e.Id == id);
        //}
    }
}
