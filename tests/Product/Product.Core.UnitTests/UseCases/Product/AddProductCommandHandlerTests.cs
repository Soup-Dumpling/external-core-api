using External.Product.Core.Services;
using External.Product.Core.UseCases.Product.AddProduct;
using External.Product.Core.UseCases.Product.GetProducts;
using NSubstitute;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace External.Product.Core.UnitTests.UseCases.Product
{
    public class AddProductCommandHandlerTests
    {
        AddProductCommandHandler addProductCommandHandler;
        IHttpService httpServiceMock = Substitute.For<IHttpService>();

        public AddProductCommandHandlerTests()
        {
            addProductCommandHandler = new AddProductCommandHandler(httpServiceMock);
        }

        [Fact]
        public async Task ValidCommand()
        {
            //Arrange
            var addProductUrl = "https://api.restful-api.dev/objects";
            var addProductCommand = new AddProductCommand("Galaxy S25 Ultra", new Data { Capacity = "512 GB", Color = "Titanium Silverblue", Price = 1349, Year = 2025 });
            var addedProduct = new AddedProductModel() { Id = "ff80818196f2a23f01977215413210f5", Name =  addProductCommand.Name, Data = addProductCommand.Data, CreatedAt = DateTime.UtcNow };
            httpServiceMock.PostAsync<AddedProductModel>(Arg.Any<string>(), Arg.Any<StringContent>()).Returns(addedProduct);

            //Act
            var result = await addProductCommandHandler.Handle(addProductCommand, CancellationToken.None);

            //Assert
            await httpServiceMock.Received().PostAsync<AddedProductModel>(addProductUrl, Arg.Any<StringContent>());
            Assert.NotNull(result);
            Assert.Equal(addedProduct.Id, result.Id);
            Assert.Equal(addedProduct.Name, result.Name);
            Assert.Equal(addedProduct.Data, result.Data);
            Assert.Equal(addedProduct.CreatedAt, result.CreatedAt);
        }
    }
}
