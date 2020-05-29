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

namespace BarCrawlers.Tests.IngredientsServiceTests
{
    [TestClass]
    public class Delete_Should
    {
        [TestMethod]
        public async Task ReturnTrue_when_ValidIngredientAsync()
        {

            //Arrange
            var options = Utils.GetOptions("ReturnTrue_when_ValidIngredientAsync");
            var entity = new Ingredient
            {
                Id = Utils.MySampleGuid(),
                Name = "Lime",
                IsAlcoholic = false
            };

            var mockMapper = new Mock<IIngredientMapper>();

            using (var arrangeContext = new BCcontext(options))
            {
                arrangeContext.Ingredients.Add(entity);
                arrangeContext.SaveChanges();
            }

            //Act & Assert
            using (var context = new BCcontext(options))
            {
                var sut = new IngredientsService(context, mockMapper.Object);

                var result = await sut.DeleteAsync(Utils.MySampleGuid());

                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        public async Task ReturnFalse_when_NotValidIngredientIDAsync()
        {
            //Arrange
            var options = Utils.GetOptions("ReturnFalse_when_NotValidIngredientIDAsync");
            var entity = new Ingredient
            {
                Name = "Lime",
                IsAlcoholic = false
            };

            var mockMapper = new Mock<IIngredientMapper>();

            using (var arrangeContext = new BCcontext(options))
            {
                arrangeContext.Ingredients.Add(entity);
                arrangeContext.SaveChanges();
            }

            //Act & Assert
            using (var context = new BCcontext(options))
            {
                var sut = new IngredientsService(context, mockMapper.Object);

                var result = await sut.DeleteAsync(Utils.MySampleGuid());

                Assert.IsFalse(result);
            }
        }

    }
}
