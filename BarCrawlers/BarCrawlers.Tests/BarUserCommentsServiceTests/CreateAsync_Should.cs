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


namespace BarCrawlers.Tests.BarUserCommentsServiceTests
{
    [TestClass]
    public class CreateComment_Should
    {
        [TestMethod]
        public async Task CreateComment_whenValid()
        {
            var testBarName = "TestBarName";
            var testUserName = "TestUserName";
            var options = Utils.GetOptions(nameof(CreateComment_whenValid));

            var testUser = new User()
            {
                Id = Utils.MySampleGuid(),
                UserName = testUserName,
            };
            var testBar = new Bar()
            {
                Id = Utils.MySampleGuid3(),
                Name = testBarName,
            };
            var dto = new BarUserCommentDTO()
            {
                UserId = testUser.Id,
                BarId = testBar.Id,
                Text = "Comment text here.",
            };

            var mockMapper = new Mock<IBarUserCommentMapper>();
            mockMapper.Setup(x => x.MapDTOToEntity(It.IsAny<BarUserCommentDTO>()))
                .Returns((BarUserCommentDTO x) => new BarUserComment()
                {
                    BarId = x.BarId,
                    UserId = x.UserId,
                    Text = x.Text
                });
            mockMapper.Setup(x => x.MapEntityToDTO(It.IsAny<BarUserComment>()))
                .Returns((BarUserComment x) => new BarUserCommentDTO()
                {
                    BarId = x.BarId,
                    BarName = x.Bar.Name,
                    UserId = x.UserId,
                    UserName = x.User.UserName,
                    Text = x.Text
                });



            using (var arrangeContext = new BCcontext(options))
            {
                await arrangeContext.Users.AddAsync(testUser);
                await arrangeContext.Bars
                    .AddAsync(testBar);
                await arrangeContext.SaveChangesAsync();
            }

            using (var actContext = new BCcontext(options))
            {
                var sut = new BarUserCommentsService(actContext, mockMapper.Object);
                var comment = sut.CreateAsync(dto);
                await actContext.SaveChangesAsync();
            }

            using (var assertContext = new BCcontext(options))
            {
                Assert.AreEqual(1, assertContext.BarComments.Count());
                var bar = await assertContext.Bars
                    .Include(x => x.Comments)
                    .FirstOrDefaultAsync(x => x.Name == testBarName);

                var comment = bar.Comments.FirstOrDefault();
                Assert.IsNotNull(comment);
                Assert.AreEqual(dto.Text, comment.Text);
            }
        }


        [TestMethod]
        public async Task CreateComment_whenAlreadyCommented()
        {
            var testBarName = "TestBarName";
            var testUserName = "TestUserName";
            var options = Utils.GetOptions(nameof(CreateComment_whenAlreadyCommented));

            var testUser = new User()
            {
                Id = Utils.MySampleGuid(),
                UserName = testUserName,
            };
            var testBar = new Bar()
            {
                Id = Utils.MySampleGuid3(),
                Name = testBarName,
            };
            var testComment = new BarUserComment()
            {
                UserId = testUser.Id,
                User = testUser,
                BarId = testBar.Id,
                Bar = testBar,
                Text = "Comment text here.",
            };
            var dto = new BarUserCommentDTO()
            {
                UserId = testUser.Id,
                BarId = testBar.Id,
                Text = "New comment text here.",
            };

            var mockMapper = new Mock<IBarUserCommentMapper>();
            mockMapper.Setup(x => x.MapDTOToEntity(It.IsAny<BarUserCommentDTO>()))
                .Returns((BarUserCommentDTO x) => new BarUserComment()
                {
                    BarId = x.BarId,
                    UserId = x.UserId,
                    Text = x.Text
                });
            mockMapper.Setup(x => x.MapEntityToDTO(It.IsAny<BarUserComment>()))
                .Returns((BarUserComment x) => new BarUserCommentDTO()
                {
                    BarId = x.BarId,
                    BarName = x.Bar.Name,
                    UserId = x.UserId,
                    UserName = x.User.UserName,
                    Text = x.Text
                });

            using (var arrangeContext = new BCcontext(options))
            {
                await arrangeContext.Users.AddAsync(testUser);
                await arrangeContext.Bars.AddAsync(testBar);
                await arrangeContext.BarComments.AddAsync(testComment);
                await arrangeContext.SaveChangesAsync();
            }

            using (var context = new BCcontext(options))
            {
                var sut = new BarUserCommentsService(context, mockMapper.Object);
                //Act
                var result = await sut.CreateAsync(dto);
                //Assert
                Assert.AreNotEqual(testBar.Id, result.BarId);
                Assert.IsNull(result.BarName);
                Assert.AreNotEqual(testUser.Id, result.UserId);
                Assert.IsNull(result.UserName);
                Assert.IsNull(result.Text);

                Assert.AreEqual(1, context.BarComments.Count());
                var bar = await context.Bars
                    .Include(x => x.Comments)
                    .FirstOrDefaultAsync(x => x.Name == testBarName);

                var comment = bar.Comments.FirstOrDefault();
                Assert.IsNotNull(comment);
                Assert.AreNotEqual(dto.Text, comment.Text);

            }
        }

    }
}
