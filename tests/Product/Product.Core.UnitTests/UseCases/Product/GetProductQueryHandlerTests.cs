using External.Product.Core.Services;
using External.Product.Core.UseCases.Product.GetProduct;
using External.Product.Core.UseCases.Product.GetProducts;
using NSubstitute;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace External.Product.Core.UnitTests.UseCases.Product
{
    public class GetProductQueryHandlerTests
    {
        GetProductQueryHandler getProductQueryHandler;
        IHttpService httpServiceMock = Substitute.For<IHttpService>();

        public GetProductQueryHandlerTests()
        {
            getProductQueryHandler = new GetProductQueryHandler(httpServiceMock);
        }

        [Fact]
        public async Task ValidQuery()
        {
            //Arrange
            var getProductQuery = new GetProductQuery("1");
            var getProductUrl = $"https://api.restful-api.dev/objects/{getProductQuery.Id}";
            var expectedProduct = new GetProductsModel() { Id = "1", Name = "Google Pixel 6 Pro", Data = new Data() { Color = "Cloudy White", Capacity = "128 GB" } };
            httpServiceMock.GetAsync<GetProductsModel>(Arg.Any<string>()).Returns(expectedProduct);

            //Act
            var result = await getProductQueryHandler.Handle(getProductQuery, CancellationToken.None);

            //Assert
            await httpServiceMock.Received().GetAsync<GetProductsModel>(getProductUrl);
            Assert.NotNull(result);
            Assert.Equal(expectedProduct, result);
        }
    }
}
