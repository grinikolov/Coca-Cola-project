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
    public class GetAllAsync_Should
    {
        [TestMethod]
        public async Task ReturnAllRecords_IfAdmin()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(ReturnAllRecords_IfAdmin));

            var barList = new List<Bar>();
            for (int i = 1; i <= 20; i++)
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

                if (i%2 == 0)
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
                var result = await sut.GetAllAsync("0", "20", string.Empty, "asc", true);

                Assert.AreEqual(result.Count(), 20);
                Assert.AreEqual(result.Any(b => b.IsDeleted == true), true);
            }
        }

        [TestMethod]
        public async Task ReturnUndeletedRecords_IfNotAdmin()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(ReturnUndeletedRecords_IfNotAdmin));

            var barList = new List<Bar>();
            for (int i = 1; i <= 20; i++)
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

                if (i % 2 == 0)
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
                var result = await sut.GetAllAsync("0", "20", string.Empty, "asc", false);

                Assert.AreEqual(result.Count(), 10);
                Assert.AreEqual(result.Any(b => b.IsDeleted == true), false);
            }
        }

        [TestMethod]
        public async Task ReturnAllAccordingSearchParameter_InNames_IfAdmin()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(ReturnAllAccordingSearchParameter_InNames_IfAdmin));

            var barList = new List<Bar>();
            for (int i = 1; i <= 20; i++)
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

                if (i % 2 == 0)
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
                var result = await sut.GetAllAsync("0", "20", "1", "asc", true);

                Assert.AreEqual(result.Count(), 11);
                Assert.AreEqual(result.Any(b => b.IsDeleted == true), true);
                Assert.AreEqual(result.Where(r => r.Name.Contains("1")).Count(), result.Count());
            }
        }

        [TestMethod]
        public async Task ReturnUndeletedAccordingSearchParameter_InNames_IfNotAdmin()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(ReturnUndeletedAccordingSearchParameter_InNames_IfNotAdmin));

            var barList = new List<Bar>();
            for (int i = 1; i <= 20; i++)
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

                if (i % 2 == 0)
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
                var result = await sut.GetAllAsync("0", "20", "1", "asc", false);

                Assert.AreEqual(result.Count(), 6);
                Assert.AreEqual(result.Any(b => b.IsDeleted == true), false);
                Assert.AreEqual(result.Where(r => r.Name.Contains("1")).Count(), result.Count());
            }
        }

        [TestMethod]
        public async Task ReturnSetNumberOfBars()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(ReturnSetNumberOfBars));

            var barList = new List<Bar>();
            for (int i = 1; i <= 20; i++)
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

                if (i % 2 == 0)
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
                var result = await sut.GetAllAsync("0", "4", string.Empty, "asc", true);

                Assert.AreEqual(result.Count(), 4);
            }
        }

        [TestMethod]
        public async Task ReturnSpecificSetOfBars()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(ReturnSpecificSetOfBars));

            var barList = new List<Bar>();
            var set = new List<Bar>();
            for (int i = 1; i <= 20; i++)
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

                if (i % 2 == 0)
                {
                    bar.IsDeleted = true;
                }

                barList.Add(bar);
            }

            set = barList.OrderBy(b => b.Name).ToList();
            set = set.Skip(4).Take(4).ToList();

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
                var result = await sut.GetAllAsync("1", "4", string.Empty, "asc", true);
                var compare = result.ToList();

                Assert.AreEqual(set[0].Name, compare[0].Name);
                Assert.AreEqual(set[1].Name, compare[1].Name);
                Assert.AreEqual(set[2].Name, compare[2].Name);
                Assert.AreEqual(set[3].Name, compare[3].Name);

            }
        }

        [TestMethod]
        public async Task ThrowArgumentException_IfParameterIncorect()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(ThrowArgumentException_IfParameterIncorect));

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

                await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await sut.GetAllAsync("a", "10", string.Empty, "asc", true));
                await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await sut.GetAllAsync("0", "a", string.Empty, "asc", true));
            }
        }

    }
}
