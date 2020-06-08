using BarCrawlers.Data;
using BarCrawlers.Data.DBModels;
using BarCrawlers.Services;
using BarCrawlers.Services.DTOs;
using BarCrawlers.Services.Mappers.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BarCrawlers.Tests.UsersServiceTests
{
    [TestClass]
    public class GetAsync_Should
    {
        [TestMethod]
        public async Task GetCorrectUser_WhenSuccessful()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(GetCorrectUser_WhenSuccessful));

            var user = new User()
            {
                UserName = "UserName1",
                Email = "user1@mail.bg",
                PasswordHash = "pass",
            };

            using (var context = new BCcontext(options))
            {
                await context.Users.AddAsync(user);
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
                var dbResult = await context.Users.FirstOrDefaultAsync(b => b.UserName == "UserName1");
                var result = await sut.GetAsync(dbResult.Id);

                Assert.AreEqual(dbResult.UserName, result.UserName);
                Assert.AreEqual(dbResult.Email, result.Email);
            }
        }

        [TestMethod]
        public async Task ReturnUserDTO_WhenGetSuccesfull()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(ReturnUserDTO_WhenGetSuccesfull));

            var user = new User()
            {
                UserName = "UserName1",
                Email = "user1@mail.bg",
                PasswordHash = "pass",
            };

            using (var context = new BCcontext(options))
            {
                await context.Users.AddAsync(user);
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
                var dbResult = await context.Users.FirstOrDefaultAsync(b => b.UserName == "UserName1");
                var result = await sut.GetAsync(dbResult.Id);

                Assert.IsInstanceOfType(result, typeof(UserDTO));
            }
        }

        [TestMethod]
        public async Task ThrowArgumentException_WhenExceptionHappens()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(ReturnUserDTO_WhenGetSuccesfull));

            var user = new User()
            {
                UserName = "UserName1",
                Email = "user1@mail.bg",
                PasswordHash = "pass",
            };

            using (var context = new BCcontext(options))
            {
                await context.Users.AddAsync(user);
                await context.SaveChangesAsync();
            }

            var mockMapper = new Mock<IUserMapper>();


            mockMapper.Setup((x) => x.MapEntityToDTO(It.IsAny<User>()))
                .Returns(() => throw new Exception());

            var coctailMapper = new Mock<ICocktailCommentMapper>();

            //Act & Assert
            using (var context = new BCcontext(options))
            {
                var sut = new UsersService(context, mockMapper.Object, coctailMapper.Object);
                var dbResult = await context.Users.FirstOrDefaultAsync(b => b.UserName == "UserName1");

                await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await sut.GetAsync(dbResult.Id));
            }
        }
    }
}
