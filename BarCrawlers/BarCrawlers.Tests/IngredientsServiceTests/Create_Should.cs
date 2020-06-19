using BarCrawlers.Data;
using BarCrawlers.Data.DBModels;
using BarCrawlers.Services;
using BarCrawlers.Services.DTOs;
using BarCrawlers.Services.Mappers.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using System.Threading.Tasks;

namespace BarCrawlers.Tests.IngredientsServiceTests
{
    [TestClass]
    public class Create_Should
    {
        [TestMethod]
        public async Task CreateIngredient_When_IngredientDoesNotExist()
        {
            var options = Utils.GetOptions(nameof(CreateIngredient_When_IngredientDoesNotExist));
            var testIngredientName = "IngredientName";

            var entity = new Ingredient()
            {
                Name = "SampleIngredientName",
                IsAlcoholic = true,

            };

            var mockMapper = new Mock<IIngredientMapper>();
            mockMapper.Setup(x => x.MapDTOToEntity(It.IsAny<IngredientDTO>()))
                .Returns((IngredientDTO i) => new Ingredient()
                {
                    Name = testIngredientName,
                    IsAlcoholic = true
                });
            mockMapper.Setup(x => x.MapEntityToDTO(It.IsAny<Ingredient>()))
                .Returns((Ingredient i) => new IngredientDTO()
                {
                    Name = testIngredientName,
                    IsAlcoholic = true
                });


            using (var arrangeContext = new BCcontext(options))
            {
                await arrangeContext.Ingredients
                    .AddAsync(entity);
                await arrangeContext.SaveChangesAsync();
            }

            using (var actContext = new BCcontext(options))
            {
                var sut = new IngredientsService(actContext, mockMapper.Object);
                var ingredient = new IngredientDTO()
                {
                    Name = testIngredientName,
                    IsAlcoholic = true,
                };
                var testIngredient = await sut.CreateAsync(ingredient);
                await actContext.SaveChangesAsync();
            }

            using (var assertContext = new BCcontext(options))
            {
                Assert.AreEqual(2, assertContext.Ingredients.Count());
                var ingredient = await assertContext.Ingredients.FirstOrDefaultAsync(x => x.Name == testIngredientName);
                Assert.IsNotNull(ingredient);
                Assert.IsTrue(ingredient.IsAlcoholic);
            }
        }
    }
}
