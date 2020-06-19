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
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BarCrawlers.Tests.BarsServiceTests
{
    [TestClass]
    public class UpdateAync_Should
    {
        [TestMethod]
        public async Task UpdatesBar_WhenSuccessful()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(UpdatesBar_WhenSuccessful));

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

            var bar = new BarDTO()
            {
                Name = "BestBarAgain",
                Rating = 4,
                TimesRated = 1,
                ImageSrc = null,
                IsDeleted = false,
                Address = "Галичица 17",
                Country = "България",
                District = "Лозенец",
                Email = "other@mail.bg",
                Phone = "088888888",
                Town = "София",
                Cocktails = new List<CocktailBarDTO>()
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
                   Town = b.Town,
               });

            var http = new Mock<IHttpClientFactory>();
            var coctailMapper = new Mock<ICocktailMapper>();

            //Act & Assert
            using (var context = new BCcontext(options))
            {
                var sut = new BarsService(context, mockMapper.Object, http.Object, coctailMapper.Object);
                var dbResult = await context.Bars.FirstOrDefaultAsync(b => b.Name == "BestBar");
                var result = await sut.UpdateAsync(dbResult.Id, bar);
                dbResult = await context.Bars.FirstOrDefaultAsync(b => b.Name == "BestBarAgain");
                var oldRecord = await context.Bars.FirstOrDefaultAsync(b => b.Name == "BestBar");

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
                Assert.AreEqual(oldRecord, null);
            }
        }

        [TestMethod]
        public async Task ReturnNull_WhenBarDoesntExist()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(ReturnNull_WhenBarDoesntExist));

            using (var context = new BCcontext(options))
            {
            }

            var bar = new BarDTO()
            {
                Name = "BestBarAgain",
                Rating = 4,
                TimesRated = 1,
                ImageSrc = null,
                IsDeleted = false,
                Address = "Галичица 17",
                Country = "България",
                District = "Лозенец",
                Email = "other@mail.bg",
                Phone = "088888888",
                Town = "София"
            };

            var mockMapper = new Mock<IBarMapper>();
            var http = new Mock<IHttpClientFactory>();
            var coctailMapper = new Mock<ICocktailMapper>();

            //Act & Assert
            using (var context = new BCcontext(options))
            {
                var sut = new BarsService(context, mockMapper.Object, http.Object, coctailMapper.Object);
                var result = await sut.UpdateAsync(Guid.NewGuid(), bar);

                Assert.AreEqual(result, null);
            }
        }

        [TestMethod]
        public async Task ThrowArgumentNullException_WhenBarDTONull()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(ThrowArgumentNullException_WhenBarDTONull));

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

                await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await sut.UpdateAsync(dbResult.Id, null));
            }
        }

        [TestMethod]
        public async Task AddCocktail_WhenSuccessful()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(AddCocktail_WhenSuccessful));

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

            var cocktail = new Cocktail()
            {
                Name = "SomeCocktail",
                Rating = 3,
                TimesRated = 1,
                ImageSrc = null,
                IsDeleted = false,
            };

            using (var context = new BCcontext(options))
            {
                await context.Bars.AddAsync(record);
                await context.Cocktails.AddAsync(cocktail);
                await context.SaveChangesAsync();
            }



            var bar = new BarDTO()
            {
                Name = "BestBarAgain",
                Rating = 4,
                TimesRated = 1,
                ImageSrc = null,
                IsDeleted = false,
                Address = "Галичица 17",
                Country = "България",
                District = "Лозенец",
                Email = "other@mail.bg",
                Phone = "088888888",
                Town = "София",
                Cocktails = new List<CocktailBarDTO>()
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
                   Town = b.Town,
               });

            var http = new Mock<IHttpClientFactory>();
            var coctailMapper = new Mock<ICocktailMapper>();

            //Act & Assert
            using (var context = new BCcontext(options))
            {
                var contextCoctail = await context.Cocktails.FirstOrDefaultAsync(c => c.Name == "SomeCocktail");
                var cocktailBarDTO = new CocktailBarDTO()
                {
                    CocktailId = contextCoctail.Id,
                    CocktailName = contextCoctail.Name,
                    Remove = false
                };
                bar.Cocktails.Add(cocktailBarDTO);
                var sut = new BarsService(context, mockMapper.Object, http.Object, coctailMapper.Object);
                var dbResultOld = await context.Bars.FirstOrDefaultAsync(b => b.Name == "BestBar");
                var result = await sut.UpdateAsync(dbResultOld.Id, bar);
                var dbResult = await context.Bars.Include(b => b.Cocktails).FirstOrDefaultAsync(b => b.Name == "BestBarAgain");

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
                Assert.AreEqual(dbResult.Cocktails.Count(), 1);
                Assert.AreEqual(dbResult.Cocktails.Where(c => c.CocktailId == contextCoctail.Id).Count(), 1);
            }
        }

        [TestMethod]
        public async Task RemoveCocktail_WhenSuccessful()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(RemoveCocktail_WhenSuccessful));

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

            var cocktail1 = new Cocktail()
            {
                Name = "SomeCocktail",
                Rating = 3,
                TimesRated = 1,
                ImageSrc = null,
                IsDeleted = false,
            };

            var cocktail2 = new Cocktail()
            {
                Name = "SomeOtherCocktail",
                Rating = 3,
                TimesRated = 1,
                ImageSrc = null,
                IsDeleted = false,
            };

            using (var context = new BCcontext(options))
            {
                await context.Cocktails.AddAsync(cocktail1);
                await context.Cocktails.AddAsync(cocktail2);
                await context.Bars.AddAsync(record);
                await context.SaveChangesAsync();
                var conection1 = new CocktailBar()
                {
                    Cocktail = await context.Cocktails.FirstOrDefaultAsync(c => c.Name == "SomeCocktail"),
                    Bar = await context.Bars.FirstOrDefaultAsync(b => b.Name == "BestBar")
                };
                var conection2 = new CocktailBar()
                {
                    Cocktail = await context.Cocktails.FirstOrDefaultAsync(c => c.Name == "SomeOtherCocktail"),
                    Bar = await context.Bars.FirstOrDefaultAsync(b => b.Name == "BestBar")
                };
                await context.CocktailBars.AddAsync(conection1);
                await context.CocktailBars.AddAsync(conection2);
                await context.SaveChangesAsync();
            }



            var bar = new BarDTO()
            {
                Name = "BestBarAgain",
                Rating = 4,
                TimesRated = 1,
                ImageSrc = null,
                IsDeleted = false,
                Address = "Галичица 17",
                Country = "България",
                District = "Лозенец",
                Email = "other@mail.bg",
                Phone = "088888888",
                Town = "София",
                Cocktails = new List<CocktailBarDTO>()
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
                   Town = b.Town,
               });

            var http = new Mock<IHttpClientFactory>();
            var coctailMapper = new Mock<ICocktailMapper>();

            //Act & Assert
            using (var context = new BCcontext(options))
            {
                var contextCoctail1 = await context.Cocktails.FirstOrDefaultAsync(c => c.Name == "SomeCocktail");
                var cocktailBarDTO1 = new CocktailBarDTO()
                {
                    CocktailId = contextCoctail1.Id,
                    CocktailName = contextCoctail1.Name,
                    Remove = false
                };
                var contextCoctail2 = await context.Cocktails.FirstOrDefaultAsync(c => c.Name == "SomeOtherCocktail");
                var cocktailBarDTO2 = new CocktailBarDTO()
                {
                    CocktailId = contextCoctail2.Id,
                    CocktailName = contextCoctail2.Name,
                    Remove = true
                };
                bar.Cocktails.Add(cocktailBarDTO1);
                bar.Cocktails.Add(cocktailBarDTO2);
                var sut = new BarsService(context, mockMapper.Object, http.Object, coctailMapper.Object);
                var dbResultOld = await context.Bars.FirstOrDefaultAsync(b => b.Name == "BestBar");
                var result = await sut.UpdateAsync(dbResultOld.Id, bar);
                var dbResult = await context.Bars.Include(b => b.Cocktails).FirstOrDefaultAsync(b => b.Name == "BestBarAgain");

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
                Assert.AreEqual(dbResult.Cocktails.Count(), 1);
                Assert.AreEqual(dbResult.Cocktails.Where(c => c.CocktailId == contextCoctail1.Id).Count(), 1);
            }
        }
    }
}
