using External.Product.Core.Services;
using External.Product.Core.UseCases.Product.GetProducts;
using External.Product.Core.UseCases.Product.UpdateProduct;
using NSubstitute;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace External.Product.Core.UnitTests.UseCases.Product
{
    public class UpdateProductCommandHandlerTests
    {
        UpdateProductCommandHandler updateProductCommandHandler;
        IHttpService httpServiceMock = Substitute.For<IHttpService>();

        public UpdateProductCommandHandlerTests()
        {
            updateProductCommandHandler = new UpdateProductCommandHandler(httpServiceMock);
        }

        [Fact]
        public async Task ValidCommand()
        {
            //Arrange
            var updateProductCommand = new UpdateProductCommand("ff80818196f2a23f01977215413210f5", "Galaxy S25 Ultra Pro", new Data { Capacity = "1 TB", Color = "Titanium Silverblue", Price = 1649, Year = 2025 });
            var updateProductUrl = $"https://api.restful-api.dev/objects/{updateProductCommand.Id}";
            var updatedProduct = new UpdatedProductModel() { Id = updateProductCommand.Id, Name = updateProductCommand.Name, Data = updateProductCommand.Data, UpdatedAt = DateTime.UtcNow };
            httpServiceMock.PutAsync<UpdatedProductModel>(Arg.Any<string>(), Arg.Any<StringContent>()).Returns(updatedProduct);

            //Act
            var result = await updateProductCommandHandler.Handle(updateProductCommand, CancellationToken.None);

            //Assert
            await httpServiceMock.Received().PutAsync<UpdatedProductModel>(updateProductUrl, Arg.Any<StringContent>());
            Assert.NotNull(result);
            Assert.Equal(updatedProduct.Id, result.Id);
            Assert.Equal(updatedProduct.Name, result.Name);
            Assert.Equal(updatedProduct.Data, result.Data);
            Assert.Equal(updatedProduct.UpdatedAt, result.UpdatedAt);
        }
    }
}
