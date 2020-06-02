using BarCrawlers.Data;
using BarCrawlers.Data.DBModels;
using BarCrawlers.Services;
using BarCrawlers.Services.Contracts;
using BarCrawlers.Services.DTOs;
using BarCrawlers.Services.Mappers.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarCrawlers.Tests.CocktailsServiceTests
{
    [TestClass]
    public class GetAll_Should
    {

        [TestMethod]
        public async Task ReturnCorrect_whenValidCocktails()
        {
            var testCocktailName = "TestCocktailName";
            var options = Utils.GetOptions(nameof(ReturnCorrect_whenValidCocktails));

            var cocktailId = Utils.MySampleGuid3();


            var entityCocktail = new Cocktail()
            {
                Id = Utils.MySampleGuid(),
                Name = testCocktailName
            };

            var sampleCocktail = new Cocktail()
            {
                Id = Utils.MySampleGuid2(),
                Name = "SampleCocktailName",
            };

            var mockMapper = new Mock<ICocktailMapper>();
            mockMapper.Setup(x => x.MapDTOToEntity(It.IsAny<CocktailDTO>()))
                .Returns((CocktailDTO x) => new Cocktail()
                {
                    Name = x.Name,
                });
            mockMapper.Setup(x => x.MapEntityToDTO(It.IsAny<Cocktail>()))
                .Returns((Cocktail x) => new CocktailDTO()
                {
                    Id = x.Id,
                    Name = x.Name,
                });


            using (var arrangeContext = new BCcontext(options))
            {
                await arrangeContext.Cocktails
                    .AddRangeAsync(entityCocktail, sampleCocktail);
                await arrangeContext.SaveChangesAsync();
            }

            using (var actContext = new BCcontext(options))
            {
                var sut = new CocktailsService(actContext, mockMapper.Object);
                var cocktails = await sut.GetAllAsync("0", "12");

                Assert.AreEqual(2, actContext.Cocktails.Count());
                Assert.IsNotNull(cocktails);
                Assert.AreEqual(sampleCocktail.Id, cocktails.ToList()[1].Id);
                Assert.AreEqual(testCocktailName, cocktails.ToList()[0].Name);
            }
        }
        //[TestMethod]
        //public async Task ReturnCorrect_withSearch_whenValidCocktails()
        //{
        //    var testCocktailName = "TestCocktailName";
        //    var options = Utils.GetOptions(nameof(ReturnCorrect_withSearch_whenValidCocktails));

        //    var cocktailId = Utils.MySampleGuid3();


        //    var entityCocktail = new Cocktail()
        //    {
        //        Id = Utils.MySampleGuid(),
        //        Name = testCocktailName
        //    };

        //    var sampleCocktail = new Cocktail()
        //    {
        //        Id = Utils.MySampleGuid2(),
        //        Name = "SampleCocktailName",
        //    };

        //    var mockMapper = new Mock<ICocktailMapper>();
        //    mockMapper.Setup(x => x.MapDTOToEntity(It.IsAny<CocktailDTO>()))
        //        .Returns((CocktailDTO x) => new Cocktail()
        //        {
        //            Name = x.Name,
        //        });
        //    mockMapper.Setup(x => x.MapEntityToDTO(It.IsAny<Cocktail>()))
        //        .Returns((Cocktail x) => new CocktailDTO()
        //        {
        //            Id = x.Id,
        //            Name = x.Name,
        //        });


        //    using (var arrangeContext = new BCcontext(options))
        //    {
        //        await arrangeContext.Cocktails
        //            .AddRangeAsync(entityCocktail, sampleCocktail);
        //        await arrangeContext.SaveChangesAsync();
        //    }

        //    using (var actContext = new BCcontext(options))
        //    {
        //        var sut = new CocktailsService(actContext, mockMapper.Object);
        //        var cocktails = await sut.GetAllAsync("0", "12","sample");

        //        Assert.AreEqual(1, actContext.Cocktails.Count());
        //        Assert.IsNotNull(cocktails);
        //        Assert.AreEqual(sampleCocktail.Id, cocktails.ToList()[1].Id);
        //        Assert.AreEqual("SampleCocktailName", cocktails.ToList()[1].Name);
        //    }
        //}
        [TestMethod]
        public async Task ReturnNull_noSearch_whenNoCocktail()
        {
            var options = Utils.GetOptions(nameof(ReturnNull_noSearch_whenNoCocktail));

            var mockMapper = new Mock<ICocktailMapper>();

            using (var arrangeContext = new BCcontext(options))
            {
            }

            using (var actContext = new BCcontext(options))
            {
                var sut = new CocktailsService(actContext, mockMapper.Object);
                var cocktail = await sut.GetAllAsync("0", "12");

                Assert.IsNotNull(cocktail);
                Assert.AreEqual(0, actContext.Cocktails.Count());
            }
        }

        [TestMethod]
        public async Task ReturnNull_noSearch_whenNoCocktails()
        {
            var options = Utils.GetOptions(nameof(ReturnNull_noSearch_whenNoCocktails));

            var mockMapper = new Mock<ICocktailMapper>();

            using (var arrangeContext = new BCcontext(options))
            {
            }

            using (var actContext = new BCcontext(options))
            {
                var sut = new CocktailsService(actContext, mockMapper.Object);
                var cocktail = await sut.GetAllAsync("0", "12","","", false);

                Assert.IsNotNull(cocktail);
                Assert.AreEqual(0, actContext.Cocktails.Count());
            }
        }

        [TestMethod]
        public async Task ReturnNull_withSearch_whenNoCocktail()
        {
            var options = Utils.GetOptions(nameof(ReturnNull_withSearch_whenNoCocktail));

            var mockMapper = new Mock<ICocktailMapper>();

            using (var arrangeContext = new BCcontext(options))
            {
            }

            using (var actContext = new BCcontext(options))
            {
                string order = "asc";
                var sut = new CocktailsService(actContext, mockMapper.Object);
                var cocktail = await sut.GetAllAsync("0", "12","searchString", order, true);

                Assert.IsNotNull(cocktail);
                Assert.AreEqual(0, actContext.Cocktails.Count());
            }
        }
    }
}
