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
    public class Get_Should
    {
        [TestMethod]
        public void ReturnCorrectIngredient_when_Valid()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(ReturnCorrectIngredient_when_Valid));
            var entity = new Ingredient
            {
                Id = Utils.MySampleGuid(),
                Name = "Lime",
                IsAlcoholic = false
            };

            var mockMapper = new Mock<IIngredientMapper>();

            mockMapper.Setup(x => x.MapEntityToDTO(entity))
                .Returns(new IngredientDTO()
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    IsAlcoholic = entity.IsAlcoholic,
                });

            using (var arrangeContext = new BCcontext(options))
            {
                arrangeContext.Ingredients.Add(entity);
                arrangeContext.SaveChanges();
            }

            //Act & Assert
            using (var context = new BCcontext(options))
            {
                var sut = new IngredientsService(context, mockMapper.Object);

                var result = sut.GetAsync(Utils.MySampleGuid()).Result;

                Assert.AreEqual(entity.Id, result.Id);
                Assert.AreEqual(entity.Name, result.Name);
                Assert.AreEqual(entity.IsAlcoholic, result.IsAlcoholic);

            }
        }
    }
}
