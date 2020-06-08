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
            this._logger = logger;
            this._barsService = barsService;
            this._cocktailsService = cocktailsService;
            this._barMapper = barMapper;
            this._cocktailMapper = cocktailMapper;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                HomeIndexViewModel homeModel = new HomeIndexViewModel();

                var bars = await this._barsService.GetBestBarsAsync();
                homeModel.TopBars = bars.Select(b => this._barMapper.MapDTOToView(b));

                var cocktails = await this._cocktailsService.GetBestCocktailsAsync();
                homeModel.TopCocktails = cocktails.Select(c => this._cocktailMapper.MapDTOToView(c));

                return View(homeModel);
            }
            catch (Exception e)
            {
                this._logger.LogError(e.Message);
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
