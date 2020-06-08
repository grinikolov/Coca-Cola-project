using BarCrawlers.Data;
using BarCrawlers.Data.DBModels;
using BarCrawlers.Services;
using BarCrawlers.Services.DTOs;
using BarCrawlers.Services.Mappers;
using BarCrawlers.Services.Mappers.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BarCrawlers.Tests.CocktailsServiceTests
{
    [TestClass]
    public class Delete_Should
    {
        [TestMethod]
        public async Task DeleteCocktail_ShouldReturnTrue_when_ValidAsync()
        {

            //Arrange
            var options = Utils.GetOptions(nameof(DeleteCocktail_ShouldReturnTrue_when_ValidAsync));

            var entity = new Cocktail
            {
                Id = Utils.MySampleGuid(),
                Name = "Mohito",
                IsDeleted = false,
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

            using (var arrangeContext = new BCcontext(options))
            {
                arrangeContext.Cocktails.Add(entity);
                arrangeContext.SaveChanges();
            }

            //Act & Assert
            using (var context = new BCcontext(options))
            {
                var sut = new CocktailsService(context, mockMapper.Object, mockBarMapper.Object);

                var result = await sut.DeleteAsync(Utils.MySampleGuid());

                Assert.IsTrue(result);
            }
        }



        [TestMethod]
        public void DeleteCocktail_ReturnFalse_when_NotValidID()
        {
            //Arrange
            var options = Utils.GetOptions("DeleteCocktail_ReturnFalse_when_NotValidID");
            //var entity = new
            var mockMapper = new Mock<ICocktailMapper>();

            var mockBarMapper = new Mock<IBarMapper>();

            //Act & Assert
            using (var context = new BCcontext(options))
            {
                var sut = new CocktailsService(context, mockMapper.Object, mockBarMapper.Object);

                var result = sut.DeleteAsync(Utils.MySampleGuid()).Result;

                Assert.IsFalse(result);
            }



        }
    }
}
