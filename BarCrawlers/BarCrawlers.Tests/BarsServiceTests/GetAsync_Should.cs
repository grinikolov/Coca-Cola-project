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
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BarCrawlers.Tests.BarsServiceTests
{
    [TestClass]
    public class GetAsync_Should
    {
        [TestMethod]
        public async Task GetCorrectBar_WhenSuccessful()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(GetCorrectBar_WhenSuccessful));

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


            mockMapper.Setup((x) => x.MapEntityToDTO(It.IsAny<Bar>()))
                .Returns((Bar b) => new BarDTO()
                {
                    Id = b.Id,
                    Name = b.Name,
                    Rating = b.Rating,
                    TimesRated = b.TimesRated,
                    ImageSrc = b.ImageSrc,
                    IsDeleted = b.IsDeleted,
                    Address = b.Address,
                    Country = b.Country,
                    District = b.District,
                    Email = b.Email,
                    LocationId = b.LocationId,
                    Phone = b.Phone,
                    Town = b.Town
                });

            var http = new Mock<IHttpClientFactory>();
            var coctailMapper = new Mock<ICocktailMapper>();

            //Act & Assert
            using (var context = new BCcontext(options))
            {
                var sut = new BarsService(context, mockMapper.Object, http.Object, coctailMapper.Object);
                var dbResult = await context.Bars.FirstOrDefaultAsync(b => b.Name == "BestBar");
                var result = await sut.GetAsync(dbResult.Id);

                Assert.AreEqual(dbResult.Name, result.Name);
                Assert.AreEqual(dbResult.Rating, result.Rating);
                Assert.AreEqual(dbResult.TimesRated, result.TimesRated);
                Assert.AreEqual(dbResult.ImageSrc, result.ImageSrc);
                Assert.AreEqual(dbResult.IsDeleted, result.IsDeleted);
                Assert.AreEqual(dbResult.Address, result.Address);
                Assert.AreEqual(dbResult.Country, result.Country);
                Assert.AreEqual(dbResult.District, result.District);
                Assert.AreEqual(dbResult.Email, result.Email);
                Assert.AreEqual(dbResult.Phone, result.Phone);
                Assert.AreEqual(dbResult.Town, result.Town);
            }
        }

        [TestMethod]
        public async Task ReturnBarDTO_WhenGetSuccesfull()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(ReturnBarDTO_WhenGetSuccesfull));

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


            mockMapper.Setup((x) => x.MapEntityToDTO(It.IsAny<Bar>()))
                .Returns((Bar b) => new BarDTO()
                {
                    Id = b.Id,
                    Name = b.Name,
                    Rating = b.Rating,
                    TimesRated = b.TimesRated,
                    ImageSrc = b.ImageSrc,
                    IsDeleted = b.IsDeleted,
                    Address = b.Address,
                    Country = b.Country,
                    District = b.District,
                    Email = b.Email,
                    LocationId = b.LocationId,
                    Phone = b.Phone,
                    Town = b.Town
                });

            var http = new Mock<IHttpClientFactory>();
            var coctailMapper = new Mock<ICocktailMapper>();


            //Act & Assert
            using (var context = new BCcontext(options))
            {
                var sut = new BarsService(context, mockMapper.Object, http.Object, coctailMapper.Object);
                var dbResult = await context.Bars.FirstOrDefaultAsync(b => b.Name == "BestBar");
                var result = await sut.GetAsync(dbResult.Id);

                Assert.IsInstanceOfType(result, typeof(BarDTO));
            }
        }

        [TestMethod]
        public async Task ThrowArgumentException_ExceptionHappens()
        {
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

            //Arrange
            var options = Utils.GetOptions(nameof(ThrowArgumentException_ExceptionHappens));
            using (var context = new BCcontext(options))
            {
                await context.Bars.AddAsync(record);
                await context.SaveChangesAsync();
            }

            var mockMapper = new Mock<IBarMapper>();


            mockMapper.Setup((x) => x.MapEntityToDTO(It.IsAny<Bar>()))
                .Returns(() => throw new ArgumentNullException());

            var http = new Mock<IHttpClientFactory>();
            var coctailMapper = new Mock<ICocktailMapper>();


            //Act & Assert
            using (var context = new BCcontext(options))
            {
                var sut = new BarsService(context, mockMapper.Object, http.Object, coctailMapper.Object);
                var dbResult = await context.Bars.FirstOrDefaultAsync(b => b.Name == "BestBar");

                await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await sut.GetAsync(dbResult.Id));
            }
        }
    }
}
