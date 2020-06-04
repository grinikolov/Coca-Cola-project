using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BarCrawlers.Models;
using BarCrawlers.Services.Contracts;
using BarCrawlers.Models.Contracts;

namespace BarCrawlers.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBarsService _service;
        private readonly IBarViewMapper _mapper;

        public HomeController(ILogger<HomeController> logger, IBarsService service, IBarViewMapper mapper)
        {
            _logger = logger;
            _service = service;
            _mapper = mapper;
        }

        public async Task<IActionResult> IndexAsync()
        {
            try
            {
                var bars = await _service.GetBestBarsAsync();
                return View(bars.Select(b => this._mapper.MapDTOToView(b)));
            }
            catch (Exception)
            {

                return RedirectToAction("Error");
            }

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
