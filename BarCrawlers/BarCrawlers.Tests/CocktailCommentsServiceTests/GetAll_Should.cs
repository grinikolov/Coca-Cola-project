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
    public class GetAll_Should
    {
        [TestMethod]
        public async Task GetRightComments_Successfuly()
        {
            var testCocktailName = "TestCocktailName";
            var testUserName = "TestUserName";
            var options = Utils.GetOptions(nameof(GetRightComments_Successfuly));

            var testUser = new User()
            {
                Id = Utils.MySampleGuid(),
                UserName = testUserName,
            };
            var testUser2 = new User()
            {
                Id = Utils.MySampleGuid2(),
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
            var testComment2 = new CocktailUserComment()
            {
                UserId = testUser2.Id,
                User = testUser2,
                CocktailId = testCocktail.Id,
                Cocktail = testCocktail,
                Text = "Comment text here.2",
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
                await arrangeContext.Users.AddAsync(testUser2);
                await arrangeContext.Cocktails.AddAsync(testCocktail);
                await arrangeContext.CocktailComments.AddAsync(testComment);
                await arrangeContext.CocktailComments.AddAsync(testComment2);
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
                Assert.AreEqual(2, context.CocktailComments.Count());
                //Act

                var result = await sut.GetAllAsync(testCocktail.Id, "0", "12");
                //Assert
                Assert.IsNotNull(result);
                //Does not change other
                Assert.AreEqual(1, context.Cocktails.Count());
                Assert.AreEqual(2, context.Users.Count());
                Assert.AreEqual(2, context.CocktailComments.Count());
                //Correct comments
                Assert.AreEqual(testCocktail.Id, result.ToList()[0].CocktailId);
                Assert.AreEqual(testUser.Id, result.ToList()[0].UserId);
                Assert.AreEqual(testCocktail.Id, result.ToList()[1].CocktailId);
                Assert.AreEqual(testUser2.Id, result.ToList()[1].UserId);
                Assert.AreEqual(testComment.Text, result.ToList()[0].Text);
                Assert.AreEqual(testComment2.Text, result.ToList()[1].Text);
            }
        }

    }
}
