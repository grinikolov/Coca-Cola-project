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
using BarCrawlers.Areas.Magician.Models;
using Microsoft.AspNetCore.Identity;
using BarCrawlers.Services.DTOs;
using BarCrawlers.Areas.Magician.Models.Contrtacts;

namespace BarCrawlers.Areas.Magician.Controllers
{
    [Area("Magician")]
    public class UserController : Controller
    {
        private readonly IUsersService _service;
        private readonly IUserViewMapper _mapper;

        public UserController(IUsersService service, IUserViewMapper mapper)
        {
            this._service = service ?? throw new ArgumentNullException("User service not found");
            this._mapper = mapper ?? throw new ArgumentNullException("User mapper not found");
        }

        // GET: Magician/User
        public async Task<IActionResult> Index(string page = "0", string itemsOnPage = "10")
        {
            try
            {
                var serviceResult = await _service.GetAllAsync(page, itemsOnPage);
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

            var user = await _service.GetAsync(id);
                //.FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
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

        //// GET: Magician/User/Edit/5
        //public async Task<IActionResult> Edit(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var user = await _context.Users.FindAsync(id);
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
        public async Task<IActionResult> Edit(Guid id, [Bind("ImageSrc,Id,UserName,Email,EmailConfirmed,PhoneNumber,PhoneNumberConfirmed,LockoutEnd,LockoutEnabled")] UserViewModel user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var userDTO = new UserDTO();
                    await _service.UpdateAsync(id, userDTO);
                }
                catch (DbUpdateConcurrencyException)
                {
                    //if (!UserExists(user.Id))
                    //{
                    //    return NotFound();
                    //}
                    //else
                    //{
                    //    throw;
                    //}
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Magician/User/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _service.GetAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
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
