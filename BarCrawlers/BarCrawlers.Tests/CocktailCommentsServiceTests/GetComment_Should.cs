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

namespace BarCrawlers.Tests.CocktailCommentsServiceTests
{
    [TestClass]
    public class GetComment_Should
    {
        [TestMethod]
        public async Task GetRightComment_Successfuly()
        {
            var testCocktailName = "TestCocktailName";
            var testUserName = "TestUserName";
            var options = Utils.GetOptions(nameof(GetRightComment_Successfuly));

            var testUser = new User()
            {
                Id = Utils.MySampleGuid(),
                UserName = testUserName,
            };
            var testCocktail = new Cocktail()
            {
                Id = Utils.MySampleGuid3(),
                Name = testCocktailName,
            };
            var testComment = new CocktailUserComment()
            {
                UserId = testUser.Id,
                User = testUser,
                CocktailId = testCocktail.Id,
                Cocktail = testCocktail,
                Text = "Comment text here.",
            };
            var dto = new CocktailUserCommentDTO()
            {
                UserId = testUser.Id,
                CocktailId = testCocktail.Id,
                Text = "Comment text here.",
            };

            var mockMapper = new Mock<ICocktailCommentMapper>();
            mockMapper.Setup(x => x.MapDTOToEntity(It.IsAny<CocktailUserCommentDTO>()))
                .Returns((CocktailUserCommentDTO x) => new CocktailUserComment()
                {
                    CocktailId = x.CocktailId,
                    UserId = x.UserId,
                    Text = x.Text
                });
            mockMapper.Setup(x => x.MapEntityToDTO(It.IsAny<CocktailUserComment>()))
                .Returns((CocktailUserComment x) => new CocktailUserCommentDTO()
                {
                    CocktailId = x.CocktailId,
                    CocktailName = x.Cocktail.Name,
                    UserId = x.UserId,
                    UserName = x.User.UserName,
                    Text = x.Text
                });

            using (var arrangeContext = new BCcontext(options))
            {
                await arrangeContext.Users.AddAsync(testUser);
                await arrangeContext.Cocktails.AddAsync(testCocktail);
                await arrangeContext.CocktailComments.AddAsync(testComment);
                await arrangeContext.SaveChangesAsync();
            }

            //Act & Assert
            using (var context = new BCcontext(options))
            {
                var sut = new CocktailCommentsService(context, mockMapper.Object);

                Assert.IsNotNull(context.Cocktails
                    .Include(x => x.Comments)
                    .FirstOrDefault(x => x.Name == testCocktailName)
                    .Comments.FirstOrDefault());
                Assert.AreEqual(1, context.CocktailComments.Count());
                //Act

                var result = await sut.GetAsync(testCocktail.Id, testUser.Id);
                //Assert
                Assert.IsNotNull(result);
                //Does not change other
                Assert.AreEqual(1, context.Cocktails.Count());
                Assert.AreEqual(1, context.Users.Count());
                Assert.AreEqual(1, context.CocktailComments.Count());
                //Correct comment
                Assert.AreEqual(testCocktail.Id, result.CocktailId);
                Assert.AreEqual(testCocktailName, result.CocktailName);
                Assert.AreEqual(testUser.Id, result.UserId);
                Assert.AreEqual(testUserName, result.UserName);
                Assert.AreEqual("Comment text here.", result.Text);
            }
        }

    }
}
