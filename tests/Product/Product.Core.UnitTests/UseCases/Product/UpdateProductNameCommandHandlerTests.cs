using External.Product.Core.Services;
using External.Product.Core.UseCases.Product.GetProducts;
using External.Product.Core.UseCases.Product.UpdateProduct;
using External.Product.Core.UseCases.Product.UpdateProductName;
using NSubstitute;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace External.Product.Core.UnitTests.UseCases.Product
{
    public class UpdateProductNameCommandHandlerTests
    {
        UpdateProductNameCommandHandler updateProductNameCommandHandler;
        IHttpService httpServiceMock = Substitute.For<IHttpService>();

        public UpdateProductNameCommandHandlerTests()
        {
            updateProductNameCommandHandler = new UpdateProductNameCommandHandler(httpServiceMock);
        }

        [Fact]
        public async Task ValidCommand()
        {
            //Arrange
            var updateProductNameCommand = new UpdateProductNameCommand("ff80818196f2a23f01977215413210f5", "Galaxy S25 Ultra Pro Max");
            var updateProductNameUrl = $"https://api.restful-api.dev/objects/{updateProductNameCommand.Id}";
            var updatedProduct = new UpdatedProductModel() { Id = updateProductNameCommand.Id, Name = updateProductNameCommand.Name, Data = new Data { Capacity = "512 GB", Color = "Titanium Silverblue", Price = 1349, Year = 2025 }, UpdatedAt = DateTime.UtcNow };
            httpServiceMock.PatchAsync<UpdatedProductModel>(Arg.Any<string>(), Arg.Any<StringContent>()).Returns(updatedProduct);

            //Act
            var result = await updateProductNameCommandHandler.Handle(updateProductNameCommand, CancellationToken.None);

            //Assert
            await httpServiceMock.Received().PatchAsync<UpdatedProductModel>(updateProductNameUrl, Arg.Any<StringContent>());
            Assert.NotNull(result);
            Assert.Equal(updatedProduct.Id, result.Id);
            Assert.Equal(updatedProduct.Name, result.Name);
            Assert.Equal(updatedProduct.Data, result.Data);
            Assert.Equal(updatedProduct.UpdatedAt, result.UpdatedAt);
        }
    }
}
