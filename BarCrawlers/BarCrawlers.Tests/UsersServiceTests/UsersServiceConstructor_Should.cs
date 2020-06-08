using BarCrawlers.Data;
using BarCrawlers.Services;
using BarCrawlers.Services.Mappers.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace BarCrawlers.Tests.UsersServiceTests
{
    [TestClass]
    public class UsersServiceConstructor_Should
    {
        [TestMethod]
        public void ThrowArgumentNullException_IfNoContext()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(ThrowArgumentNullException_IfNoContext));

            using (var context = new BCcontext(options))
            {
            }

            var mockMapper = new Mock<IUserMapper>();
            var coctailMapper = new Mock<ICocktailCommentMapper>();

            //Act & Assert
            using (var context = new BCcontext(options))
            {
                Assert.ThrowsException<ArgumentNullException>(() => new UsersService(null, mockMapper.Object, coctailMapper.Object));
            }
        }

        [TestMethod]
        public void ThrowArgumentNullException_IfNoCocktailMapper()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(ThrowArgumentNullException_IfNoCocktailMapper));

            using (var context = new BCcontext(options))
            {
            }

            var mockMapper = new Mock<IUserMapper>();
            var coctailMapper = new Mock<ICocktailCommentMapper>();

            //Act & Assert
            using (var context = new BCcontext(options))
            {
                Assert.ThrowsException<ArgumentNullException>(() => new UsersService(context, null, coctailMapper.Object));
            }
        }

        [TestMethod]
        public void ThrowArgumentNullException_IfNoCocktailCommentMapperforUser()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(ThrowArgumentNullException_IfNoCocktailCommentMapperforUser));

            using (var context = new BCcontext(options))
            {
            }

            var mockMapper = new Mock<IUserMapper>();
            var coctailMapper = new Mock<ICocktailCommentMapper>();

            //Act & Assert
            using (var context = new BCcontext(options))
            {
                Assert.ThrowsException<ArgumentNullException>(() => new UsersService(context, mockMapper.Object, null));
            }
        }
    }
}
