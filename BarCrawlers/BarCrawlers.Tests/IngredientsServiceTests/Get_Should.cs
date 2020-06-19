using BarCrawlers.Data;
using BarCrawlers.Data.DBModels;
using BarCrawlers.Services;
using BarCrawlers.Services.DTOs;
using BarCrawlers.Services.Mappers.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BarCrawlers.Tests.IngredientsServiceTests
{
    [TestClass]
    public class Get_Should
    {
        [TestMethod]
        public async System.Threading.Tasks.Task ReturnCorrectIngredient_when_ValidAsync()
        {
            //Arrange
            var options = Utils.GetOptions("ReturnCorrectIngredient_when_ValidAsync");
            var entity = new Ingredient
            {
                Id = Utils.MySampleGuid(),
                Name = "Lime",
                IsAlcoholic = false
            };

            var mockMapper = new Mock<IIngredientMapper>();


            mockMapper.Setup((x) => x.MapEntityToDTO(It.IsAny<Ingredient>()))
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

                var dbResult = await context.Ingredients.FirstOrDefaultAsync(x => x.Name == entity.Name);
                var result = await sut.GetAsync(dbResult.Id);

                Assert.AreEqual(dbResult.Id, result.Id);
                Assert.AreEqual(dbResult.Name, result.Name);
                Assert.AreEqual(dbResult.IsAlcoholic, result.IsAlcoholic);

            }
        }
    }
}
