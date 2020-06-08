
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
    public class Update_Should
    {
        [TestMethod]
        public async Task UpdateCocktail_When_Valid()
        {
            var testCocktailName = "TestCocktailName";
            var options = Utils.GetOptions(nameof(UpdateCocktail_When_Valid));


            var entityCocktail = new Cocktail()
            {
                Id = Utils.MySampleGuid(),
                Name = testCocktailName,
                TimesRated = 3
            };

            var ingredient = new Ingredient()
            {
                Id = Utils.MySampleGuid3(),
                IsAlcoholic = true,
            };

            var mockMapper = new Mock<ICocktailMapper>();
            mockMapper.Setup(x => x.MapDTOToEntity(It.IsAny<CocktailDTO>()))
                .Returns((CocktailDTO x) => new Cocktail()
                {
                    Id = x.Id,
                    Name = x.Name,
                    TimesRated = x.TimesRated,

                });
            mockMapper.Setup(x => x.MapEntityToDTO(It.IsAny<Cocktail>()))
                .Returns((Cocktail x) => new CocktailDTO()
                {
                    Id = x.Id,
                    Name = x.Name,
                    TimesRated = x.TimesRated,
                });
            var mockBarMapper = new Mock<IBarMapper>();


            var dto = new CocktailDTO()
            {
                Id = Utils.MySampleGuid(),
                Name = "NewCocktailName",
                IsAlcoholic = true,
                TimesRated = 4,
                Ingredients = new List<CocktailIngredientDTO>()
                { new CocktailIngredientDTO()
                {
                    IngredientId = Utils.MySampleGuid3(),
                    Parts = 2}
                },
            };

            using (var arrangeContext = new BCcontext(options))
            {
                await arrangeContext.Ingredients.AddAsync(ingredient);
                await arrangeContext.Cocktails
                    .AddAsync(entityCocktail);
                await arrangeContext.SaveChangesAsync();
            }

            using (var actContext = new BCcontext(options))
            {
                var sut = new CocktailsService(actContext, mockMapper.Object, mockBarMapper.Object);
                var cocktail = await sut.UpdateAsync(Utils.MySampleGuid(), dto);
                await actContext.SaveChangesAsync();
            }

            using (var assertContext = new BCcontext(options))
            {
                var cocktail = await assertContext.Cocktails
                    .FirstOrDefaultAsync(x => x.Id == Utils.MySampleGuid());
                Assert.IsNotNull(cocktail);
                Assert.AreEqual("NewCocktailName", cocktail.Name);
                Assert.AreEqual(dto.TimesRated, cocktail.TimesRated);
            }
        }

        //[TestMethod]
        //public async Task Throw_whenUnsuccesfull()
        //{
        //    var options = Utils.GetOptions(nameof(Throw_whenUnsuccesfull));

        //    var testId = Utils.MySampleGuid();
        //    var testCocktail = new Cocktail
        //    {
        //        Id = testId,
        //        Name = "TestCocktailName",
        //    };

        //    var mockMapper = new Mock<ICocktailMapper>();
        //    var mockBarMapper = new Mock<IBarMapper>();


        //    using (var arrangeContext = new BCcontext(options))
        //    {
        //        await arrangeContext.Cocktails.AddAsync(testCocktail);
        //        await arrangeContext.SaveChangesAsync();
        //    }

        //    using (var context = new BCcontext(options))
        //    {
        //        var sut = new CocktailsService(context, mockMapper.Object, mockBarMapper.Object);

        //        var cocktail = await context.Cocktails.FirstAsync(x => x.Id == testId);

        //        Assert.ThrowsException<ArgumentNullException>(async () => await sut.UpdateAsync(testId, null));
        //    }
        //}


    }
}
