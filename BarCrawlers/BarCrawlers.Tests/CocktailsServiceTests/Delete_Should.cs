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
        //[TestMethod]
        //public async Task DeleteCocktail_ShouldReturnTrue_when_ValidAsync()
        //{

        //    //Arrange
        //    var options = Utils.GetOptions(nameof(DeleteCocktail_ShouldReturnTrue_when_ValidAsync));
        //    // TODO: Create some ingredients
        //    var mockIngredient1 = new Mock<Ingredient>();
        //    mockIngredient1.Setup(x=> x.Id).Returns(Utils.MySampleGuid2());
            
        //    var mockIngredient2 = new Mock<Ingredient>();
        //    mockIngredient2.Setup(x => x.Id).Returns(Utils.MySampleGuid3());

        //    var entity = new Cocktail
        //    {
        //        Id = Utils.MySampleGuid(),
        //        Name = "Mohito",
                
        //    };
            
        //    var mockMapper = new Mock<ICocktailMapper>();

        //    using (var arrangeContext = new BCcontext(options))
        //    {

        //        arrangeContext.Cocktails.Add(entity);
        //        arrangeContext.SaveChanges();
        //    }

        //    //Act & Assert
        //    using (var context = new BCcontext(options))
        //    {
        //        var sut = new CocktailsService(context, mockMapper.Object);

        //        var result = await sut.DeleteAsync(Utils.MySampleGuid());

        //        Assert.IsTrue(result);
        //    }
        //}



        [TestMethod]
        public void DeleteCocktail_ReturnFalse_when_NotValidID()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(DeleteCocktail_ReturnFalse_when_NotValidID));
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
