using External.Product.Core.Services;
using External.Product.Core.UseCases.Product.DeleteProduct;
using NSubstitute;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace External.Product.Core.UnitTests.UseCases.Product
{
    public class DeleteProductCommandHandlerTests
    {
        DeleteProductCommandHandler deleteProductCommandHandler;
        IHttpService httpServiceMock = Substitute.For<IHttpService>();

        public DeleteProductCommandHandlerTests()
        {
            deleteProductCommandHandler = new DeleteProductCommandHandler(httpServiceMock);
        }

        [Fact]
        public async Task ValidCommand()
        {
            //Arrange
            var deleteProductCommand = new DeleteProductCommand("ff80818196f2a23f01977215413210f5");
            var deleteProductUrl = $"https://api.restful-api.dev/objects/{deleteProductCommand.Id}";
            var deletedProductResponse = new DeletedProductModel() { Message = $"Object with id = {deleteProductCommand.Id} has been deleted." };
            httpServiceMock.DeleteAsync<DeletedProductModel>(Arg.Any<string>()).Returns(deletedProductResponse);

            //Act
            var result = await deleteProductCommandHandler.Handle(deleteProductCommand, CancellationToken.None);

            //Assert
            await httpServiceMock.Received().DeleteAsync<DeletedProductModel>(deleteProductUrl);
            Assert.NotNull(result);
            Assert.Equal(deletedProductResponse.Message, result.Message);
        }
    }
}
