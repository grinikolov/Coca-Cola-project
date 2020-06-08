using BarCrawlers.Data;
using BarCrawlers.Data.DBModels;
using BarCrawlers.Services;
using BarCrawlers.Services.DTOs;
using BarCrawlers.Services.Mappers.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BarCrawlers.Tests.BarsServiceTests
{
    [TestClass]
    public class GetCocktailsAsync1Params_Should
    {
        [TestMethod]
        public async Task GetBarCocktails_WhenSuccessful()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(GetBarCocktails_WhenSuccessful));

            var record = new Bar()
            {
                Name = "BestBar",
                Rating = 4,
                TimesRated = 1,
                ImageSrc = null,
                IsDeleted = false,
                Address = "Галичица 17.",
                Country = "България.",
                District = "Лозенец.",
                Email = "some@mail.bg",
                Phone = "088888888.",
                Town = "София.",
                LocationId = null,
            };

            var cocktailList = new List<Cocktail>();
            for (int i = 1; i <= 20; i++)
            {
                var cocktail = new Cocktail()
                {
                    Name = "BestCocktail" + i,
                    Rating = 4,
                    TimesRated = 1,
                    ImageSrc = null,
                    IsDeleted = false,
                    IsAlcoholic = false
                };

                cocktailList.Add(cocktail);
            }

            using (var context = new BCcontext(options))
            {
                await context.Bars.AddAsync(record);
                await context.Cocktails.AddRangeAsync(cocktailList);
                await context.SaveChangesAsync();
                var bar = await context.Bars.FirstOrDefaultAsync();
                var cocktails = await context.Cocktails.ToListAsync();
                foreach (var item in cocktails)
                {
                    var join = new CocktailBar()
                    {
                        Bar = bar,
                        Cocktail = item,
                    };

                    await context.CocktailBars.AddAsync(join);
                }

                await context.SaveChangesAsync();
            }

            var mockMapper = new Mock<IBarMapper>();
            var http = new Mock<IHttpClientFactory>();
            var coctailMapper = new Mock<ICocktailMapper>();



            coctailMapper.Setup((x) => x.MapEntityToDTO(It.IsAny<Cocktail>()))
                .Returns((Cocktail b) => new CocktailDTO()
                {
                    Id = b.Id,
                    Name = b.Name,
                    Rating = b.Rating,
                    TimesRated = b.TimesRated,
                    ImageSrc = b.ImageSrc,
                    IsDeleted = b.IsDeleted,
                    IsAlcoholic = b.IsAlcoholic
                });

            //Act & Assert
            using (var context = new BCcontext(options))
            {
                var sut = new BarsService(context, mockMapper.Object, http.Object, coctailMapper.Object);
                var dbResult = await context.CocktailBars
                    .Include(c => c.Cocktail)
                    .ToListAsync();
                
                var dbBar = await context.Bars.FirstOrDefaultAsync();
                var result = await sut.GetCocktailsAsync(dbBar.Id);

                Assert.AreEqual(result.Count(), dbResult.Count());
                foreach (var item in dbResult)
                {
                    Assert.IsNotNull(result.FirstOrDefault(r => r.Name == item.Cocktail.Name));
                }    
            }
        }
    }
}
