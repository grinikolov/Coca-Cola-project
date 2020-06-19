using BarCrawlers.Data;
using BarCrawlers.Data.DBModels;
using BarCrawlers.Services;
using BarCrawlers.Services.DTOs;
using BarCrawlers.Services.Mappers.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Net.Http;
using System.Threading.Tasks;
//using Microsoft.Extensions.DependencyModel;

namespace BarCrawlers.Tests.BarsServiceTests
{
    [TestClass]
    public class CreateAsync_Should
    {
        [TestMethod]
        public async Task RecordsBar_WhenBarDoesntExist()
        {
            //Arrange
            var options = Utils.GetOptions("RecordsBar_WhenBarDoesntExist");


            using (var context = new BCcontext(options))
            {
            }

            var bar = new BarDTO()
            {
                Name = "BestBar",
                Rating = 4,
                TimesRated = 1,
                ImageSrc = null,
                IsDeleted = false,
                Address = "Галичица 17",
                Country = "България",
                District = "Лозенец",
                Email = "some@mail.bg",
                Phone = "088888888",
                Town = "София"
            };

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

            mockMapper.Setup((x) => x.MapDTOToEntity(It.IsAny<BarDTO>()))
               .Returns((BarDTO b) => new Bar()
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
                await sut.CreateAsync(bar);

                var dbResult = await context.Bars.FirstOrDefaultAsync(x => x.Name == bar.Name);

                Assert.AreEqual(dbResult.Name, bar.Name);
                Assert.AreEqual(dbResult.Rating, bar.Rating);
                Assert.AreEqual(dbResult.TimesRated, bar.TimesRated);
                Assert.AreEqual(dbResult.ImageSrc, bar.ImageSrc);
                Assert.AreEqual(dbResult.IsDeleted, bar.IsDeleted);
                Assert.AreEqual(dbResult.Address, bar.Address);
                Assert.AreEqual(dbResult.Country, bar.Country);
                Assert.AreEqual(dbResult.District, bar.District);
                Assert.AreEqual(dbResult.Email, bar.Email);
                Assert.AreEqual(dbResult.Phone, bar.Phone);
                Assert.AreEqual(dbResult.Town, bar.Town);

            }
        }

        [TestMethod]
        public async Task UndeletesBar_WhenBarExistAsync()
        {
            //Arrange
            var options = Utils.GetOptions("UndeletesBar_WhenBarExist");

            var record = new Bar()
            {
                Name = "BestBar",
                Rating = 4,
                TimesRated = 1,
                ImageSrc = null,
                IsDeleted = true,
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

            var bar = new BarDTO()
            {
                Name = "BestBar",
                Rating = 4,
                TimesRated = 1,
                ImageSrc = null,
                IsDeleted = false,
                Address = "Галичица 17",
                Country = "България",
                District = "Лозенец",
                Email = "some@mail.bg",
                Phone = "088888888",
                Town = "София"
            };

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

            mockMapper.Setup((x) => x.MapDTOToEntity(It.IsAny<BarDTO>()))
               .Returns((BarDTO b) => new Bar()
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
                await sut.CreateAsync(bar);

                var dbResult = await context.Bars.FirstOrDefaultAsync(x => x.Name == bar.Name);

                Assert.AreEqual(dbResult.Name, record.Name);
                Assert.AreEqual(dbResult.Rating, record.Rating);
                Assert.AreEqual(dbResult.TimesRated, record.TimesRated);
                Assert.AreEqual(dbResult.ImageSrc, record.ImageSrc);
                Assert.AreEqual(dbResult.IsDeleted, false);
                Assert.AreEqual(dbResult.Address, record.Address);
                Assert.AreEqual(dbResult.Country, record.Country);
                Assert.AreEqual(dbResult.District, record.District);
                Assert.AreEqual(dbResult.Email, record.Email);
                Assert.AreEqual(dbResult.Phone, record.Phone);
                Assert.AreEqual(dbResult.Town, record.Town);
            }
        }

        [TestMethod]
        public async Task ReturnBarDTO_WhenSuccesfull()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(ReturnBarDTO_WhenSuccesfull));


            using (var context = new BCcontext(options))
            {
            }

            var bar = new BarDTO()
            {
                Name = "BestBar",
                Rating = 4,
                TimesRated = 1,
                ImageSrc = null,
                IsDeleted = false,
                Address = "Галичица 17",
                Country = "България",
                District = "Лозенец",
                Email = "some@mail.bg",
                Phone = "088888888",
                Town = "София"
            };

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

            mockMapper.Setup((x) => x.MapDTOToEntity(It.IsAny<BarDTO>()))
               .Returns((BarDTO b) => new Bar()
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
                var result = await sut.CreateAsync(bar);

                var dbResult = await context.Bars.FirstOrDefaultAsync(x => x.Name == bar.Name);

                Assert.IsInstanceOfType(result, typeof(BarDTO));
            }
        }

        [TestMethod]
        public async Task ThrowArgumentNullException_WhenArgumentNull()
        {
            //Arrange
            var options = Utils.GetOptions("RecordsBar_WhenBarDoesntExist");


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

                await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await sut.CreateAsync(null));
            }
        }
    }
}
