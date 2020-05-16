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

namespace BarCrawlers.Tests.CocktailsServiceTests
{
    [TestClass]
    public class Delete_Should
    {
        [TestMethod]
        public void DeleteCocktail_ShouldReturnTrue_when_Valid()
        {

            //Arrange
            var options = Utils.GetOptions(nameof(DeleteCocktail_ShouldReturnTrue_when_Valid));
            // TODO: Create some ingredients
           // var ingredient1 = new Ingredient { Id = Some Guid};

            // save them to the database
            var entity = new Cocktail
            {



              //  Ingredients = 
            };


            var mockMapper = new Mock<ICocktailMapper>();

            using (var arrangeContext = new BCcontext(options))
            {
                arrangeContext.Cocktails.Add(entity);
                arrangeContext.SaveChanges();
            }

            //Act & Assert
            using (var context = new BCcontext(options))
            {
                var sut = new CocktailsService(context, mockMapper.Object);

                var result = sut.DeleteAsync(Utils.MySampleGuid()).Result;

                Assert.IsTrue(result);
            }
        }



        [TestMethod]
        public void DeleteCocktail_ReturnFalse_when_NotValidIngredientID()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(DeleteCocktail_ReturnFalse_when_NotValidIngredientID));
            //var entity = new
            var mockMapper = new Mock<ICocktailMapper>();

            //Act & Assert
            using (var context = new BCcontext(options))
            {
                var sut = new CocktailsService(context, mockMapper.Object);

                var result = sut.DeleteAsync(Utils.MySampleGuid()).Result;

                Assert.IsFalse(result);
            }



        }
    }
}
