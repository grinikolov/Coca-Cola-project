using BarCrawlers.Data;
using BarCrawlers.Services;
using BarCrawlers.Services.Mappers.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Net.Http;

namespace BarCrawlers.Tests.BarsServiceTests
{
    [TestClass]
    public class BarServiceConstructor_Should
    {
        [TestMethod]
        public void ThrowArgumentNullException_IfNoContext()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(ThrowArgumentNullException_IfNoContext));

            using (var context = new BCcontext(options))
            {
            }

            var mockMapper = new Mock<IBarMapper>();
            var http = new Mock<IHttpClientFactory>();
            var coctailMapper = new Mock<ICocktailMapper>();

            //Act & Assert
            using (var context = new BCcontext(options))
            {
                Assert.ThrowsException<ArgumentNullException>(() => new BarsService(null, mockMapper.Object, http.Object, coctailMapper.Object));
            }
        }

        [TestMethod]
        public void ThrowArgumentNullException_IfNoBarMapper()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(ThrowArgumentNullException_IfNoBarMapper));

            using (var context = new BCcontext(options))
            {
            }

            var http = new Mock<IHttpClientFactory>();
            var coctailMapper = new Mock<ICocktailMapper>();

            //Act & Assert
            using (var context = new BCcontext(options))
            {
                Assert.ThrowsException<ArgumentNullException>(() => new BarsService(context, null, http.Object, coctailMapper.Object));
            }
        }

        [TestMethod]
        public void ThrowArgumentNullException_IfNoHttpClient()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(ThrowArgumentNullException_IfNoHttpClient));

            using (var context = new BCcontext(options))
            {
            }

            var mockMapper = new Mock<IBarMapper>();
            var coctailMapper = new Mock<ICocktailMapper>();

            //Act & Assert
            using (var context = new BCcontext(options))
            {
                Assert.ThrowsException<ArgumentNullException>(() => new BarsService(context, mockMapper.Object, null, coctailMapper.Object));
            }
        }

        [TestMethod]
        public void ThrowArgumentNullException_IfNoCocktailMapperforBar()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(ThrowArgumentNullException_IfNoCocktailMapperforBar));

            using (var context = new BCcontext(options))
            {
            }

            var mockMapper = new Mock<IBarMapper>();
            var http = new Mock<IHttpClientFactory>();

            //Act & Assert
            using (var context = new BCcontext(options))
            {
                Assert.ThrowsException<ArgumentNullException>(() => new BarsService(context, mockMapper.Object, http.Object, null));
            }
        }
    }
}
