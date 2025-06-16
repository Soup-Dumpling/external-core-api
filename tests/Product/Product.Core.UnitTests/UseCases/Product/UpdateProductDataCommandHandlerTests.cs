using External.Product.Core.Services;
using External.Product.Core.UseCases.Product.GetProducts;
using External.Product.Core.UseCases.Product.UpdateProduct;
using External.Product.Core.UseCases.Product.UpdateProductData;
using NSubstitute;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace External.Product.Core.UnitTests.UseCases.Product
{
    public class UpdateProductDataCommandHandlerTests
    {
        UpdateProductDataCommandHandler updateProductDataCommandHandler;
        IHttpService httpServiceMock = Substitute.For<IHttpService>();

        public UpdateProductDataCommandHandlerTests()
        {
            updateProductDataCommandHandler = new UpdateProductDataCommandHandler(httpServiceMock);
        }

        [Fact]
        public async Task ValidCommand()
        {
            //Arrange
            var updateProductDataCommand = new UpdateProductDataCommand("ff80818196f2a23f01977215413210f5", new Data { Capacity = "1 TB", Color = "Titanium Silverblue", Price = 1599, Year = 2025 });
            var updateProductDataUrl = $"https://api.restful-api.dev/objects/{updateProductDataCommand.Id}";
            var updatedProduct = new UpdatedProductModel() { Id = updateProductDataCommand.Id, Name = "Galaxy S25 Ultra", Data = updateProductDataCommand.Data, UpdatedAt = DateTime.UtcNow };
            httpServiceMock.PatchAsync<UpdatedProductModel>(Arg.Any<string>(), Arg.Any<StringContent>()).Returns(updatedProduct);

            //Act
            var result = await updateProductDataCommandHandler.Handle(updateProductDataCommand, CancellationToken.None);

            //Assert
            await httpServiceMock.Received().PatchAsync<UpdatedProductModel>(updateProductDataUrl, Arg.Any<StringContent>());
            Assert.NotNull(result);
            Assert.Equal(updatedProduct.Id, result.Id);
            Assert.Equal(updatedProduct.Name, result.Name);
            Assert.Equal(updatedProduct.Data, result.Data);
            Assert.Equal(updatedProduct.UpdatedAt, result.UpdatedAt);
        }
    }
}
