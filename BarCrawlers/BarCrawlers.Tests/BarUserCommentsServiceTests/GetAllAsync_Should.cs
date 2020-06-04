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

namespace BarCrawlers.Tests.BarUserCommentsServiceTests
{
    [TestClass]
    public class GetAllAsync_Should
    {
        [TestMethod]
        public async Task GetRightBarComments_Successfuly()
        {
            var testBarName = "TestBarName";
            var testUserName = "TestUserName";
            var options = Utils.GetOptions(nameof(GetRightBarComments_Successfuly));

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
            var testComment2 = new BarUserComment()
            {
                UserId = testUser2.Id,
                User = testUser2,
                BarId = testBar.Id,
                Bar = testBar,
                Text = "Comment text here.2",
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
                await arrangeContext.Users.AddAsync(testUser2);
                await arrangeContext.Bars.AddAsync(testBar);
                await arrangeContext.BarComments.AddAsync(testComment);
                await arrangeContext.BarComments.AddAsync(testComment2);
                await arrangeContext.SaveChangesAsync();
            }

            //Act & Assert
            using (var context = new BCcontext(options))
            {
                var sut = new BarUserCommentsService(context, mockMapper.Object);

                Assert.IsNotNull(context.Bars
                    .Include(x => x.Comments)
                    .FirstOrDefault(x => x.Name == testBarName)
                    .Comments.FirstOrDefault());
                Assert.AreEqual(2, context.BarComments.Count());
                //Act

                var result = await sut.GetAllAsync(testBar.Id, "0", "12");
                //Assert
                Assert.IsNotNull(result);
                //Does not change other
                Assert.AreEqual(1, context.Bars.Count());
                Assert.AreEqual(2, context.Users.Count());
                Assert.AreEqual(2, context.BarComments.Count());
                //Correct comments
                Assert.AreEqual(testBar.Id, result.ToList()[0].BarId);
                Assert.AreEqual(testUser.Id, result.ToList()[0].UserId);
                Assert.AreEqual(testBar.Id, result.ToList()[1].BarId);
                Assert.AreEqual(testUser2.Id, result.ToList()[1].UserId);
                Assert.AreEqual(testComment.Text, result.ToList()[0].Text);
                Assert.AreEqual(testComment2.Text, result.ToList()[1].Text);
            }
        }

    }
}

