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
    public class CreateComment_Should
    {
        [TestMethod]
        public async Task CreateCocktailComment_whenValid()
        {
            var testCocktailName = "TestCocktailName";
            var testUserName = "TestUserName";
            var options = Utils.GetOptions(nameof(CreateCocktailComment_whenValid));

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
                await arrangeContext.Cocktails
                    .AddAsync(testCocktail);
                await arrangeContext.SaveChangesAsync();
            }

            using (var actContext = new BCcontext(options))
            {
                var sut = new CocktailCommentsService(actContext, mockMapper.Object);
                var comment = sut.CreateAsync(dto);
                await actContext.SaveChangesAsync();
            }

            using (var assertContext = new BCcontext(options))
            {
                Assert.AreEqual(1, assertContext.CocktailComments.Count());
                var cocktail = await assertContext.Cocktails
                    .Include(x => x.Comments)
                    .FirstOrDefaultAsync(x => x.Name == testCocktailName);

                var comment = cocktail.Comments.FirstOrDefault();
                Assert.IsNotNull(comment);
                Assert.AreEqual(dto.Text, comment.Text);
            }
        }


        [TestMethod]
        public async Task CreateCocktailComment_whenAlreadyCommented()
        {
            var testCocktailName = "TestCocktailName";
            var testUserName = "TestUserName";
            var options = Utils.GetOptions(nameof(CreateCocktailComment_whenAlreadyCommented));

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
                Text = "New comment text here.",
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

            using (var context = new BCcontext(options))
            {
                var sut = new CocktailCommentsService(context, mockMapper.Object);
                //Act
                var result = await sut.CreateAsync(dto);
                //Assert
                Assert.AreNotEqual(testCocktail.Id, result.CocktailId);
                Assert.IsNull(result.CocktailName);
                Assert.AreNotEqual(testUser.Id, result.UserId);
                Assert.IsNull(result.UserName);
                Assert.IsNull(result.Text);

                Assert.AreEqual(1, context.CocktailComments.Count());
                var cocktail = await context.Cocktails
                    .Include(x => x.Comments)
                    .FirstOrDefaultAsync(x => x.Name == testCocktailName);

                var comment = cocktail.Comments.FirstOrDefault();
                Assert.IsNotNull(comment);
                Assert.AreNotEqual(dto.Text, comment.Text);

            }
        }

    }
}
