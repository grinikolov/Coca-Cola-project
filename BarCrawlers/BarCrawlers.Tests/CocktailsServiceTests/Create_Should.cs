using BarCrawlers.Data;
using BarCrawlers.Data.DBModels;
using BarCrawlers.Services;
using BarCrawlers.Services.Contracts;
using BarCrawlers.Services.DTOs;
using BarCrawlers.Services.Mappers.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarCrawlers.Tests.CocktailsServiceTests
{
    [TestClass]
    public class Create_Should
    {
        [TestMethod]
        public async Task CreateCocktail_When_ItDoesNotExist()
        {
            var testCocktailName = "TestCocktailName";
            var options = Utils.GetOptions(nameof(CreateCocktail_When_ItDoesNotExist));

            var ingredient1 = new Ingredient()
            {
                Id = Utils.MySampleGuid(),
                Name = "SampleIngredientName",
                IsAlcoholic = true,
            };
            var ingredient2 = new Ingredient()
            {
                Id = Utils.MySampleGuid(),
                Name = "SampleIngredientName",
                IsAlcoholic = true,
            };

            var entityCocktail = new Cocktail()
            { Name = "SampleCocktailName" };

            var mockMapper = new Mock<ICocktailMapper>();
            mockMapper.Setup(x => x.MapDTOToEntity(It.IsAny<CocktailDTO>()))
                .Returns((CocktailDTO x) => new Cocktail()
                {
                    Name = testCocktailName,
                });
            mockMapper.Setup(x => x.MapEntityToDTO(It.IsAny<Cocktail>()))
                .Returns((Cocktail i) => new CocktailDTO()
                {
                    Name = testCocktailName,
                });


            var dto = new CocktailDTO()
            {
                Name = testCocktailName,
                IsAlcoholic = true,
                Ingredients = new List<CocktailIngredientDTO>()
                { new CocktailIngredientDTO()
                {
                    IngredientId = Utils.MySampleGuid3(),
                    Parts = 2}
                },
            };
            var ingredient = new Ingredient()
            {
                Id = Utils.MySampleGuid3(),
                IsAlcoholic = true,
            };

            var sampleEntity = new Cocktail()
            { Name = "TestName", IsAlcoholic = false, IsDeleted = true };
            using (var arrangeContext = new BCcontext(options))
            {
                await arrangeContext.Ingredients.AddAsync(ingredient);
                await arrangeContext.Cocktails
                    .AddAsync(entityCocktail);
                await arrangeContext.SaveChangesAsync();
            }

            using (var actContext = new BCcontext(options))
            {
                var sut = new CocktailsService(actContext, mockMapper.Object);
                var cocktail = sut.CreateAsync(dto);
                await actContext.SaveChangesAsync();

            }

            using (var assertContext = new BCcontext(options))
            {
                Assert.AreEqual(2, assertContext.Cocktails.Count());
                var cocktail = await assertContext.Cocktails
                    .FirstOrDefaultAsync(x => x.Name == testCocktailName);
                Assert.IsNotNull(cocktail);
                Assert.AreEqual(testCocktailName, cocktail.Name);
                Assert.IsTrue(cocktail.IsAlcoholic);

            }
        }
        [TestMethod]
        public async Task RecoverCocktail_When_ItIsDeleted()
        {
            var testCocktailName = "TestCocktailName";
            var options = Utils.GetOptions(nameof(RecoverCocktail_When_ItIsDeleted));

            var entityCocktail = new Cocktail()
            {
                Name = testCocktailName,
                IsDeleted = true,
            };

            var dto = new CocktailDTO()
            {
                Name = testCocktailName,
            };

            var mockMapper = new Mock<ICocktailMapper>();
            mockMapper.Setup(x => x.MapDTOToEntity(It.IsAny<CocktailDTO>()))
                .Returns((CocktailDTO x) => new Cocktail()
                {
                    Name = testCocktailName,
                });
            mockMapper.Setup(x => x.MapEntityToDTO(It.IsAny<Cocktail>()))
                .Returns((Cocktail i) => new CocktailDTO()
                {
                    Name = testCocktailName,
                });



            using (var arrangeContext = new BCcontext(options))
            {
                await arrangeContext.Cocktails
                    .AddAsync(entityCocktail);
                await arrangeContext.SaveChangesAsync();
            }

            using (var actContext = new BCcontext(options))
            {
                var sut = new CocktailsService(actContext, mockMapper.Object);
                var cocktail = sut.CreateAsync(dto);
                await actContext.SaveChangesAsync();

            }

            using (var assertContext = new BCcontext(options))
            {
                var sut = new CocktailsService(assertContext, mockMapper.Object);
                var cocktail = await sut.CreateAsync(dto);
                Assert.AreEqual(1, assertContext.Cocktails.Count());
                
                Assert.IsNotNull(cocktail);
                Assert.AreEqual(testCocktailName, testCocktailName);
                Assert.IsFalse(cocktail.IsDeleted);

            }
        }

        [TestMethod]
        public async Task AddIngredientsToCocktail_True_whenValid()
        {
            var options = Utils.GetOptions(nameof(AddIngredientsToCocktail_True_whenValid));

            var cocktail = new Cocktail()
            {
                Id = Utils.MySampleGuid(),
            };
            var ingredientId = Utils.MySampleGuid2();
            var mockMapper = new Mock<ICocktailMapper>();

            using (var arrangeContext = new BCcontext(options))
            {
            }

            using (var context = new BCcontext(options))
            {
                var sut = new CocktailsService(context, mockMapper.Object);
                var result = await sut.AddIngredientsToCocktail(cocktail, ingredientId, 2);

                Assert.IsTrue(result);
            }
        }
        [TestMethod]
        public async Task AddIngredientsToCocktail_False_whenNotValid()
        {

            var options = Utils.GetOptions(nameof(AddIngredientsToCocktail_False_whenNotValid));

            var cocktail = new Cocktail()
            {
                Id = Utils.MySampleGuid(),
            };

            var ingredient = new Ingredient() { Id = Utils.MySampleGuid2() };
            var entity = new CocktailIngredient()
            {
                IngredientId = ingredient.Id,
                Ingredient = ingredient,
                CocktailId = cocktail.Id,
                Cocktail = cocktail,
                Parts = 2
            };
            var mockMapper = new Mock<ICocktailMapper>();

            using (var arrangeContext = new BCcontext(options))
            {
                await arrangeContext.Ingredients.AddAsync(ingredient);
                await arrangeContext.CocktailIngredients.AddAsync(entity);
                await arrangeContext.SaveChangesAsync();
            }

            using (var context = new BCcontext(options))
            {
                var sut = new CocktailsService(context, mockMapper.Object);
                var result = await sut.AddIngredientsToCocktail(cocktail, ingredient.Id, 2);

                Assert.IsFalse(result);
            }
        }


    }
}
