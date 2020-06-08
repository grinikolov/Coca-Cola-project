using BarCrawlers.Data;
using BarCrawlers.Data.DBModels;
using BarCrawlers.Services;
using BarCrawlers.Services.DTOs;
using BarCrawlers.Services.Mappers.Contracts;
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
    public class GetBestBarsAsync_Should
    {
        [TestMethod]
        public async Task ReturnsOnlyUndeletedRecords()
        {
            var options = Utils.GetOptions(nameof(ReturnsOnlyUndeletedRecords));

            var barList = new List<Bar>();
            for (int i = 1; i <= 3; i++)
            {
                var bar = new Bar()
                {
                    Name = "BestBar" + i,
                    Rating = 4,
                    TimesRated = 1,
                    ImageSrc = null,
                    IsDeleted = false,
                    Address = "Street " + i,
                    Country = "България",
                    District = "District " + i,
                    Email = "some@mail.bg",
                    Phone = "+ " + i + "8888888",
                    Town = "София",
                    LocationId = null,
                };

                if (i % 2 != 0)
                {
                    bar.IsDeleted = true;
                }

                barList.Add(bar);
            }

            using (var context = new BCcontext(options))
            {
                await context.Bars.AddRangeAsync(barList);
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
                var result = await sut.GetBestBarsAsync();

                Assert.AreEqual(result.Count(), 1);
                Assert.AreEqual(result.Any(b => b.IsDeleted == true), false);
            }
        }

        [TestMethod]
        public async Task ReturnsMostRatedInOrderOfRating()
        {
            var options = Utils.GetOptions(nameof(ReturnsMostRatedInOrderOfRating));

            var barList = new List<Bar>();
            for (int i = 1; i <= 3; i++)
            {
                var bar = new Bar()
                {
                    Name = "BestBar" + i,
                    Rating = 3,
                    TimesRated = i,
                    ImageSrc = null,
                    IsDeleted = false,
                    Address = "Street " + i,
                    Country = "България",
                    District = "District " + i,
                    Email = "some@mail.bg",
                    Phone = "+ " + i + "8888888",
                    Town = "София",
                    LocationId = null,
                };

                barList.Add(bar);
            }

            using (var context = new BCcontext(options))
            {
                await context.Bars.AddRangeAsync(barList);
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
                var result = await sut.GetBestBarsAsync();
                var ordered = result.OrderBy(o => o.TimesRated).ThenByDescending(o => o.Rating).ToList();

                Assert.AreEqual(result.Count(), 3);
                CollectionAssert.AreEqual(result.ToList(), ordered);
            }
        }

        [TestMethod]
        public async Task ThrowsArgumentExceptionWhenFailedToGetBestBars()
        {
            var options = Utils.GetOptions(nameof(ThrowsArgumentExceptionWhenFailedToGetBestBars));

            var barList = new List<Bar>();
            for (int i = 1; i <= 3; i++)
            {
                var bar = new Bar()
                {
                    Name = "BestBar" + i,
                    Rating = 3,
                    TimesRated = i,
                    ImageSrc = null,
                    IsDeleted = false,
                    Address = "Street " + i,
                    Country = "България",
                    District = "District " + i,
                    Email = "some@mail.bg",
                    Phone = "+ " + i + "8888888",
                    Town = "София",
                    LocationId = null,
                };

                barList.Add(bar);
            }

            using (var context = new BCcontext(options))
            {
                await context.Bars.AddRangeAsync(barList);
                await context.SaveChangesAsync();
            }

            var mockMapper = new Mock<IBarMapper>();


            mockMapper.Setup((x) => x.MapEntityToDTO(It.IsAny<Bar>()))
                .Returns(() => throw new Exception());

            var http = new Mock<IHttpClientFactory>();
            var coctailMapper = new Mock<ICocktailMapper>();

            //Act & Assert
            using (var context = new BCcontext(options))
            {
                var sut = new BarsService(context, mockMapper.Object, http.Object, coctailMapper.Object);

                await Assert.ThrowsExceptionAsync<ArgumentException> (async () =>  await sut.GetBestBarsAsync(), "Failed to get list");
            }
        }
    }
}
