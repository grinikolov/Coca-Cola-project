using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BarCrawlers.Tests.UsersServiceTests
{
    [TestClass]
    public class UpdateAsync_Should
    {
        //[TestMethod]
        //public async Task ShouldBanUser_WhenSuccessful()
        //{
        //    //Arrange
        //    var options = Utils.GetOptions(nameof(ShouldBanUser_WhenSuccessful));

        //    var user = new User()
        //    {
        //        UserName = "UserName1",
        //        Email = "user1@mail.bg",
        //        PasswordHash = "pass",
        //    };

        //    using (var context = new BCcontext(options))
        //    {
        //        await context.Users.AddAsync(user);
        //        await context.SaveChangesAsync();
        //    }

        //    var mockMapper = new Mock<IUserMapper>();


        //    mockMapper.Setup((x) => x.MapEntityToDTO(It.IsAny<User>()))
        //        .Returns((User u) => new UserDTO()
        //        {
        //            Id = u.Id,
        //            UserName = u.UserName,
        //            Email = u.Email,
        //        });

        //    var coctailMapper = new Mock<ICocktailCommentMapper>();

        //    //Act & Assert
        //    using (var context = new BCcontext(options))
        //    {
        //        var sut = new UsersService(context, mockMapper.Object, coctailMapper.Object);
        //        var dbResult = await context.Users.FirstOrDefaultAsync(b => b.UserName == "UserName1");
        //        var result = await sut.UpdateAsync(dbResult.Id);

        //        Assert.AreEqual(dbResult.UserName, result.UserName);
        //        Assert.AreEqual(dbResult.Email, result.Email);
        //    }
        //}
    }
}
