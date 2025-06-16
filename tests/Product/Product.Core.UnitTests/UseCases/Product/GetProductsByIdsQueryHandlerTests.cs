using External.Product.Core.Services;
using External.Product.Core.UseCases.Product.GetProducts;
using External.Product.Core.UseCases.Product.GetProductsByIds;
using NSubstitute;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace External.Product.Core.UnitTests.UseCases.Product
{
    public class GetProductsByIdsQueryHandlerTests
    {
        GetProductsByIdsQueryHandler getProductsByIdsQueryHandler;
        IHttpService httpServiceMock = Substitute.For<IHttpService>();

        public GetProductsByIdsQueryHandlerTests()
        {
            getProductsByIdsQueryHandler = new GetProductsByIdsQueryHandler(httpServiceMock);
        }

        [Fact]
        public async Task ValidQuery()
        {
            //Arrange
            var getProductsByIdsUrl = "https://api.restful-api.dev/objects?id=3&id=5&id=10";
            var getProductsByIdsQuery = new GetProductsByIdsQuery(["3", "5", "10"]);
            IEnumerable<GetProductsModel> expectedProducts = new List<GetProductsModel>()
            {
                new() { Id = "3", Name = "Apple iPhone 12 Pro Max", Data = new Data() { Color = "Cloudy White", CapacityGB = 512 } },
                new() { Id = "5", Name = "Samsung Galaxy Z Fold2", Data = new Data() { Color = "Brown", Price = 689.99 } },
                new() { Id = "10", Name = "Apple iPad Mini 5th Gen", Data = new Data() { Capacity = "64 GB", ScreenSize = 7.9 } }
            };
            httpServiceMock.GetAsync<IEnumerable<GetProductsModel>>(Arg.Any<string>()).Returns(expectedProducts);

            //Act
            var result = await getProductsByIdsQueryHandler.Handle(getProductsByIdsQuery, CancellationToken.None);

            //Assert
            await httpServiceMock.Received().GetAsync<IEnumerable<GetProductsModel>>(getProductsByIdsUrl);
            Assert.NotNull(result);
            Assert.Equal(expectedProducts.First().Id, result.First().Id);
            Assert.Equal(expectedProducts.First().Name, result.First().Name);
            Assert.Equal(expectedProducts.First().Data, result.First().Data);
            Assert.Equal(expectedProducts.Last().Id, result.Last().Id);
            Assert.Equal(expectedProducts.Last().Name, result.Last().Name);
            Assert.Equal(expectedProducts.Last().Data, result.Last().Data);
            Assert.Equivalent(expectedProducts, result);
        }
    }
}
