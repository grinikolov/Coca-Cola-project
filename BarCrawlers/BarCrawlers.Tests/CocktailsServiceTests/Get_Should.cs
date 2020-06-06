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
    public class Get_Should
    {

        [TestMethod]
        public async Task ReturnCorrect_whenValidCocktail()
        {
            var testCocktailName = "TestCocktailName";
            var options = Utils.GetOptions(nameof(ReturnCorrect_whenValidCocktail));

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

            var mockBarMapper = new Mock<IBarMapper>();


            using (var arrangeContext = new BCcontext(options))
            {
                await arrangeContext.Cocktails
                    .AddRangeAsync(entityCocktail, sampleCocktail);
                await arrangeContext.SaveChangesAsync();
            }

            using (var actContext = new BCcontext(options))
            {
                var sut = new CocktailsService(actContext, mockMapper.Object, mockBarMapper.Object);
                var cocktail = await sut.GetAsync(Utils.MySampleGuid());

                Assert.AreEqual(2, actContext.Cocktails.Count());
                Assert.IsNotNull(cocktail);
                Assert.AreEqual(entityCocktail.Id, cocktail.Id);
                Assert.AreEqual(testCocktailName, cocktail.Name);
            }
        }
        //[TestMethod]
        //public async Task ReturnNull_whenNoCocktail()
        //{
        //    var options = Utils.GetOptions(nameof(ReturnNull_whenNoCocktail));

        //    var mockMapper = new Mock<ICocktailMapper>();

        //    var mockBarMapper = new Mock<IBarMapper>();

        //    var cocktailId = Utils.MySampleGuid3();

        //    var sampleEntity = new Cocktail()
        //    { Name = "TestName", IsAlcoholic = false, IsDeleted = true };
        //    using (var arrangeContext = new BCcontext(options))
        //    {
        //        await arrangeContext.Cocktails
        //            .AddAsync(sampleEntity);
        //        await arrangeContext.SaveChangesAsync();
        //    }

        //    using (var actContext = new BCcontext(options))
        //    {
        //        var sut = new CocktailsService(actContext, mockMapper.Object, mockBarMapper.Object);
        //        var cocktail = await sut.GetAsync(cocktailId);

        //        Assert.AreEqual(1, actContext.Cocktails.Count());
        //        Assert.IsNull(cocktail);
        //    }
        //}

    }
}
