using BarCrawlers.Data;
using BarCrawlers.Data.DBModels;
using BarCrawlers.Services;
using BarCrawlers.Services.DTOs;
using BarCrawlers.Services.Mappers.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarCrawlers.Tests.UsersServiceTests
{
    [TestClass]
    public class GetAllAsync_Should
    {

        [TestMethod]
        public async Task ReturnSetAccordingSearchParameter_InNames_IfAdmin()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(ReturnSetAccordingSearchParameter_InNames_IfAdmin));

            var userList = new List<User>();
            for (int i = 1; i <= 20; i++)
            {
                var user = new User()
                {
                    UserName = "UserName" + i,
                    Email = "user@mail.bg" + i,
                    PasswordHash = "pass",
                };
                userList.Add(user);
            }

            using (var context = new BCcontext(options))
            {
                await context.Users.AddRangeAsync(userList);
                await context.SaveChangesAsync();
            }

            var mockMapper = new Mock<IUserMapper>();


            mockMapper.Setup((x) => x.MapEntityToDTO(It.IsAny<User>()))
                .Returns((User u) => new UserDTO()
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Email = u.Email,
                });

            var coctailMapper = new Mock<ICocktailCommentMapper>();

            //Act & Assert
            using (var context = new BCcontext(options))
            {
                var sut = new UsersService(context, mockMapper.Object, coctailMapper.Object);
                var result = await sut.GetAllAsync("0", "20", "1");


                Assert.AreEqual(result.Count(), 11);
            }
        }


        [TestMethod]
        public async Task ReturnSetNumberOfUsers()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(ReturnSetNumberOfUsers));

            var userList = new List<User>();
            for (int i = 1; i <= 20; i++)
            {
                var user = new User()
                {
                    UserName = "UserName" + i,
                    Email = "user@mail.bg" + i,
                    PasswordHash = "pass",
                };
                userList.Add(user);
            }

            using (var context = new BCcontext(options))
            {
                await context.Users.AddRangeAsync(userList);
                await context.SaveChangesAsync();
            }

            var mockMapper = new Mock<IUserMapper>();


            mockMapper.Setup((x) => x.MapEntityToDTO(It.IsAny<User>()))
                .Returns((User u) => new UserDTO()
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Email = u.Email,
                });

            var coctailMapper = new Mock<ICocktailCommentMapper>();

            //Act & Assert
            using (var context = new BCcontext(options))
            {
                var sut = new UsersService(context, mockMapper.Object, coctailMapper.Object);
                var result = await sut.GetAllAsync("0", "5", "1");


                Assert.AreEqual(result.Count(), 5);
            }
        }


        [TestMethod]
        public async Task ThrowArgumentException_IfParameterIncorect()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(ThrowArgumentException_IfParameterIncorect));

            using (var context = new BCcontext(options))
            {
            }

            var mockMapper = new Mock<IUserMapper>();
            var coctailMapper = new Mock<ICocktailCommentMapper>();

            //Act & Assert
            using (var context = new BCcontext(options))
            {
                var sut = new UsersService(context, mockMapper.Object, coctailMapper.Object);

                await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await sut.GetAllAsync("a", "10", string.Empty));
                await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await sut.GetAllAsync("0", "a", string.Empty));
            }
        }

    }
}
