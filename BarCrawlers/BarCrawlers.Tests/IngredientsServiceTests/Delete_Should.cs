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

namespace BarCrawlers.Tests.IngredientsServiceTests
{
    [TestClass]
    public class Delete_Should
    {
        [TestMethod]
        public void ReturnTrue_when_ValidIngredient()
        {

            //Arrange
            var options = Utils.GetOptions(nameof(ReturnTrue_when_ValidIngredient));
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

                var result = sut.DeleteAsync(Utils.MySampleGuid()).Result;

                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        public void ReturnFalse_when_NotValidIngredientID()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(ReturnFalse_when_NotValidIngredientID));
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

                var result = sut.DeleteAsync(Utils.MySampleGuid()).Result;

                Assert.IsFalse(result);
            }
        }

    }
}
