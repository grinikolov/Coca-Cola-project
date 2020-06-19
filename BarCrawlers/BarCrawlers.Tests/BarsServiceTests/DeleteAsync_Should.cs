using BarCrawlers.Data;
using BarCrawlers.Data.DBModels;
using BarCrawlers.Services;
using BarCrawlers.Services.Mappers.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace BarCrawlers.Tests.BarsServiceTests
{
    [TestClass]
    public class DeleteAsync_Should
    {
        [TestMethod]
        public async Task ReturnFalse_WhenBarDoesntExist()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(ReturnFalse_WhenBarDoesntExist));


            using (var context = new BCcontext(options))
            {
            }

            var mockMapper = new Mock<IBarMapper>();
            var http = new Mock<IHttpClientFactory>();
            var coctailMapper = new Mock<ICocktailMapper>();

            //Act & Assert
            using (var context = new BCcontext(options))
            {
                var sut = new BarsService(context, mockMapper.Object, http.Object, coctailMapper.Object);
                var result = await sut.DeleteAsync(Guid.NewGuid());

                Assert.AreEqual(result, false);
            }
        }

        [TestMethod]
        public async Task ReturnTrue_WhenBarDeleted()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(ReturnTrue_WhenBarDeleted));

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

            using (var context = new BCcontext(options))
            {
                await context.Bars.AddAsync(record);
                await context.SaveChangesAsync();
            }

            var mockMapper = new Mock<IBarMapper>();
            var http = new Mock<IHttpClientFactory>();
            var coctailMapper = new Mock<ICocktailMapper>();

            //Act & Assert
            using (var context = new BCcontext(options))
            {
                var sut = new BarsService(context, mockMapper.Object, http.Object, coctailMapper.Object);
                var dbResult = await context.Bars.FirstOrDefaultAsync(b => b.Name == "BestBar");
                var result = await sut.DeleteAsync(dbResult.Id);
                dbResult = await context.Bars.FirstOrDefaultAsync(b => b.Name == "BestBar");

                Assert.AreEqual(result, true);
                Assert.AreEqual(dbResult.IsDeleted, true);

            }
        }
    }
}
