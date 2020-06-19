using BarCrawlers.Models;
using BarCrawlers.Models.Contracts;
using BarCrawlers.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BarCrawlers.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBarsService _barsService;
        private readonly IBarViewMapper _barMapper;
        private readonly ICocktailsService _cocktailsService;
        private readonly ICocktailViewMapper _cocktailMapper;

        public HomeController(ILogger<HomeController> logger
            , IBarsService barsService
            , ICocktailsService cocktailsService
            , IBarViewMapper barMapper
            , ICocktailViewMapper cocktailMapper)
        {
            _logger = logger;
            _barsService = barsService;
            _cocktailsService = cocktailsService;
            _barMapper = barMapper;
            _cocktailMapper = cocktailMapper;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                HomeIndexViewModel homeModel = new HomeIndexViewModel();

                var bars = await _barsService.GetBestBarsAsync();
                homeModel.TopBars = bars.Select(b => _barMapper.MapDTOToView(b));

                var cocktails = await _cocktailsService.GetBestCocktailsAsync();
                homeModel.TopCocktails = cocktails.Select(c => _cocktailMapper.MapDTOToView(c));

                return View(homeModel);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction("Error");
            }

        }
        public IActionResult Missing()
        {
            return View();
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
