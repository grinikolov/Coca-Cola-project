using BarCrawlers.Data;
using BarCrawlers.Data.DBModels;
using BarCrawlers.Services;
using BarCrawlers.Services.Mappers.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace BarCrawlers.Tests.CocktailsServiceTests
{
    [TestClass]
    public class Service_Itsrlf_Should
    {

        [TestMethod]
        public async Task CocktailExists_True_whenExisting()
        {
            var options = Utils.GetOptions(nameof(CocktailExists_True_whenExisting));
            var testCocktailId = Utils.MySampleGuid();
            var cocktail = new Cocktail() { Id = testCocktailId };

            var mockMapper = new Mock<ICocktailMapper>();

            var mockBarMapper = new Mock<IBarMapper>();

            using (var arrangeContext = new BCcontext(options))
            {
                await arrangeContext.Cocktails.AddAsync(cocktail);
                await arrangeContext.SaveChangesAsync();
            }

            //Act and Assert
            using (var context = new BCcontext(options))
            {
                var sut = new CocktailsService(context, mockMapper.Object, mockBarMapper.Object);

                var result = sut.CocktailExists(testCocktailId);
                Assert.IsTrue(result);
            }
        }


        [TestMethod]
        public void CocktailExists_False_whenNotExisting()
        {
            var options = Utils.GetOptions(nameof(CocktailExists_False_whenNotExisting));
            var testCocktailId = Utils.MySampleGuid();
            var mockMapper = new Mock<ICocktailMapper>();

            var mockBarMapper = new Mock<IBarMapper>();

            using (var arrangeContext = new BCcontext(options))
            {
            }

            //Act and Assert
            using (var context = new BCcontext(options))
            {
                var sut = new CocktailsService(context, mockMapper.Object, mockBarMapper.Object);

                var result = sut.CocktailExists(testCocktailId);
                Assert.IsFalse(result);
            }
        }
        [TestMethod]
        public async Task CocktailExistsByName_True_whenExisting()
        {
            var options = Utils.GetOptions(nameof(CocktailExistsByName_True_whenExisting));
            var mockMapper = new Mock<ICocktailMapper>();
            var mockBarMapper = new Mock<IBarMapper>();

            var testCocktailName = "TestCocktailName";
            var cocktail = new Cocktail() { Name = testCocktailName };
            using (var arrangeContext = new BCcontext(options))
            {
                await arrangeContext.Cocktails.AddAsync(cocktail);
                await arrangeContext.SaveChangesAsync();
            }

            //Act and Assert
            using (var context = new BCcontext(options))
            {
                var sut = new CocktailsService(context, mockMapper.Object, mockBarMapper.Object);

                var result = await sut.CocktailExistsByNameAsync("TestCocktailName");
                Assert.IsTrue(result);
            }
        }
        [TestMethod]
        public async Task CocktailExistsByName_False_whenNotExisting()
        {
            var options = Utils.GetOptions(nameof(CocktailExistsByName_False_whenNotExisting));
            var mockMapper = new Mock<ICocktailMapper>();
            var mockBarMapper = new Mock<IBarMapper>();


            //Act and Assert
            using (var context = new BCcontext(options))
            {
                var sut = new CocktailsService(context, mockMapper.Object, mockBarMapper.Object);

                var result = await sut.CocktailExistsByNameAsync("TestName");
                Assert.IsFalse(result);
            }
        }

    }
}
