using BarCrawlers.Data;
using BarCrawlers.Data.DBModels;
using BarCrawlers.Services;
using BarCrawlers.Services.DTOs;
using BarCrawlers.Services.Mappers;
using BarCrawlers.Services.Mappers.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BarCrawlers.Tests.IngredientsServiceTests
{
    [TestClass]
    public class GetAll_Should
    {
        [TestMethod]
        public void ReturnCorrectIngredients_when_AllValid()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(ReturnCorrectIngredients_when_AllValid)); ;
            var entity1 = new Ingredient
            {
                Name = "Lime",
                IsAlcoholic = false
            };
            var entity2 = new Ingredient
            {
                Name = "Coca Cola",
                IsAlcoholic = false
            };
            var entity3 = new Ingredient
            {
                Name = "Bacardi Oro",
                IsAlcoholic = true
            };

            var entity1DTO = new IngredientDTO
            {
                Name = "Lime",
                IsAlcoholic = false
            };
            var entity2DTO = new IngredientDTO
            {
                Name = "Coca Cola",
                IsAlcoholic = false
            };
            var entity3DTO = new IngredientDTO
            {
                Name = "Bacardi Oro",
                IsAlcoholic = true
            };

            var mockMapper = new Mock<IIngredientMapper>();


            mockMapper
                .Setup((x) => x.MapEntityToDTO(It.Is<Ingredient>(a => a.Name == entity1.Name)))
                .Returns(entity1DTO);
            mockMapper
                .Setup((x) => x.MapEntityToDTO(It.Is<Ingredient>(a => a.Name == entity2.Name)))
                .Returns(entity2DTO);
            mockMapper
                .Setup((x) => x.MapEntityToDTO(It.Is<Ingredient>(a => a.Name == entity3.Name)))
                .Returns(entity3DTO);

            using (var arrangeContext = new BCcontext(options))
            {
                arrangeContext.Ingredients.Add(entity1);
                arrangeContext.Ingredients.Add(entity2);
                arrangeContext.Ingredients.Add(entity3);
                arrangeContext.SaveChanges();
            }

            //Act & Assert
            using (var context = new BCcontext(options))
            {
                var sut = new IngredientsService(context, mockMapper.Object);

                var result =  sut.GetAllAsync().Result.ToList();

                Assert.AreEqual(3, result.Count());
                Assert.AreEqual(entity3.Name, result[2].Name);
                Assert.AreEqual(entity3.IsAlcoholic, result[2].IsAlcoholic);

            }
        }
    }
}
