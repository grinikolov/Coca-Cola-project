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

namespace BarCrawlers.Tests.BarsServiceTests
{
    [TestClass]
    public class RateBarAsync_Should
    {
        [TestMethod]
        public async Task SetRating_WhenSuccessful()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(SetRating_WhenSuccessful));

            var record = new Bar()
            {
                Name = "BestBar",
                Rating = 2,
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

            var user1 = new User()
            {
                UserName = "UserName1",
                Email = "user1@mail.bg",
                PasswordHash = "pass",
            };

            var user2 = new User()
            {
                UserName = "UserName2",
                Email = "user2@mail.bg",
                PasswordHash = "pass",
            };

            using (var context = new BCcontext(options))
            {
                await context.Bars.AddAsync(record);
                await context.Users.AddAsync(user1);
                await context.Users.AddAsync(user2);
                await context.SaveChangesAsync();

                var barRating = new UserBarRating();
                barRating.Bar = await context.Bars.FirstOrDefaultAsync(b => b.Name == "BestBar");
                barRating.User = await context.Users.FirstOrDefaultAsync(u => u.UserName == "UserName1");
                barRating.Rating = 2;

                await context.BarRatings.AddAsync(barRating);
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
                var dbUser = await context.Users.FirstOrDefaultAsync(u => u.UserName == "UserName2");
                var result = await sut.RateBarAsync(dbResult.Id, dbUser.Id, 4);
                dbResult = await context.Bars.FirstOrDefaultAsync(b => b.Name == "BestBar");

                Assert.AreEqual(dbResult.Rating, 3);
            }
        }

        [TestMethod]
        public async Task ChangeTimesRated_WhenSuccessful()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(ChangeTimesRated_WhenSuccessful));

            var record = new Bar()
            {
                Name = "BestBar",
                Rating = 0,
                TimesRated = 0,
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

            var user = new User()
            {
                UserName = "UserName",
                Email = "user@mail.bg",
                PasswordHash = "pass",
            };

            using (var context = new BCcontext(options))
            {
                await context.Bars.AddAsync(record);
                await context.Users.AddAsync(user);
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
                var dbUser = await context.Users.FirstOrDefaultAsync(u => u.UserName == "UserName");
                var result = await sut.RateBarAsync(dbResult.Id, dbUser.Id, 4);
                dbResult = await context.Bars.FirstOrDefaultAsync(b => b.Name == "BestBar");

                Assert.AreEqual(dbResult.TimesRated, 1);
            }
        }

        [TestMethod]
        public async Task RecordRating_WhenSuccessful()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(RecordRating_WhenSuccessful));

            var record = new Bar()
            {
                Name = "BestBar",
                Rating = 0,
                TimesRated = 0,
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

            var user = new User()
            {
                UserName = "UserName",
                Email = "user@mail.bg",
                PasswordHash = "pass",
            };

            using (var context = new BCcontext(options))
            {
                await context.Bars.AddAsync(record);
                await context.Users.AddAsync(user);
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
                var dbUser = await context.Users.FirstOrDefaultAsync(u => u.UserName == "UserName");
                var result = await sut.RateBarAsync(dbResult.Id, dbUser.Id, 4);
                var dbRatings = await context.BarRatings.ToListAsync();

                Assert.AreEqual(dbRatings.Count, 1);
            }
        }

        [TestMethod]
        public async Task ChangeRating_IfAlreadyRatedButNotTimesRated()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(ChangeRating_IfAlreadyRatedButNotTimesRated));

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

            var user = new User()
            {
                UserName = "UserName",
                Email = "user@mail.bg",
                PasswordHash = "pass",
            };

            using (var context = new BCcontext(options))
            {
                await context.Bars.AddAsync(record);
                await context.Users.AddAsync(user);
                await context.SaveChangesAsync();

                var barRating = new UserBarRating();
                barRating.Bar = await context.Bars.FirstOrDefaultAsync(b => b.Name == "BestBar");
                barRating.User = await context.Users.FirstOrDefaultAsync(u => u.UserName == "UserName");
                barRating.Rating = 4;

                await context.BarRatings.AddAsync(barRating);
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
                var dbUser = await context.Users.FirstOrDefaultAsync(u => u.UserName == "UserName");
                var result = await sut.RateBarAsync(dbResult.Id, dbUser.Id, 2);
                dbResult = await context.Bars.FirstOrDefaultAsync(b => b.Name == "BestBar");

                Assert.AreEqual(dbResult.TimesRated, 1);
                Assert.AreEqual(dbResult.Rating, 2);
            }
        }

        [TestMethod]
        public async Task ThrowArgumentException_IfExceptionOccurs()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(ThrowArgumentException_IfExceptionOccurs));

            var record = new Bar()
            {
                Name = "BestBar",
                Rating = 0,
                TimesRated = 0,
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

            var user = new User()
            {
                UserName = "UserName",
                Email = "user@mail.bg",
                PasswordHash = "pass",
            };

            using (var context = new BCcontext(options))
            {
                await context.Bars.AddAsync(record);
                await context.Users.AddAsync(user);
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
                var dbResult = await context.Bars.FirstOrDefaultAsync(b => b.Name == "BestBar");
                var dbUser = await context.Users.FirstOrDefaultAsync(u => u.UserName == "UserName");

                await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await sut.RateBarAsync(dbResult.Id, dbUser.Id, 4));
            }
        }
    }
}
