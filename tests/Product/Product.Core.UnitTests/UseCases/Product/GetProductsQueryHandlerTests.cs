using External.Product.Core.Models;
using External.Product.Core.Services;
using External.Product.Core.UseCases.Product.GetProducts;
using NSubstitute;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace External.Product.Core.UnitTests.UseCases.Product
{
    public class GetProductsQueryHandlerTests
    {
        GetProductsQueryHandler getProductsQueryHandler;
        IHttpService httpServiceMock = Substitute.For<IHttpService>();

        public GetProductsQueryHandlerTests() 
        {
            getProductsQueryHandler = new GetProductsQueryHandler(httpServiceMock);
        }

        [Fact]
        public async Task ValidQuery()
        {
            //Arrange
            var getProductsUrl = "https://api.restful-api.dev/objects";
            var getProductsQuery = new GetProductsQuery(null, 10, 1);
            IEnumerable<GetProductsModel> products = new List<GetProductsModel>()
            {
                new() { Id = "1", Name = "Google Pixel 6 Pro", Data = new Data() { Color = "Cloudy White", Capacity = "128 GB" } },
                new() { Id = "2", Name = "Apple iPhone 12 Mini, 256GB, Blue" },
                new() { Id = "3", Name = "Apple iPhone 12 Pro Max", Data = new Data() { Color = "Cloudy White", CapacityGB = 512 } },
                new() { Id = "4", Name = "Apple iPhone 11, 64GB", Data = new Data() { Color = "Purple", Price = 389.99 } },
                new() { Id = "5", Name = "Samsung Galaxy Z Fold2", Data = new Data() { Color = "Brown", Price = 689.99 } },
                new() { Id = "6", Name = "Apple AirPods", Data = new Data() { Generation = "3rd", Price = 120 } },
                new() { Id = "7", Name = "Apple MacBook Pro 16", Data = new Data() { CPUModel = "Intel Core i9", HardDiskSize = "1 TB", Price = 1849.99, Year = 2019 } },
                new() { Id = "8", Name = "Apple Watch Series 8", Data = new Data() { CaseSize = "41mm", StrapColour = "Elderberry" } },
                new() { Id = "9", Name = "Beats Studio3 Wireless", Data = new Data() { Color = "Red", Description = "High-performance wireless noise cancelling headphones" } },
                new() { Id = "10", Name = "Apple iPad Mini 5th Gen", Data = new Data() { Capacity = "64 GB", ScreenSize = 7.9 } },
                new() { Id = "11", Name = "Apple iPad Mini 5th Gen", Data = new Data() { Capacity = "254 GB", ScreenSize = 7.9 } },
                new() { Id = "12", Name = "Apple iPad Air", Data = new Data() { Capacity = "64 GB", Generation = "4th", Price = 419.99 } },
                new() { Id = "13", Name = "Apple iPad Air", Data = new Data() { Capacity = "256 GB", Generation = "4th", Price = 519.99 } }
            };
            httpServiceMock.GetAsync<IEnumerable<GetProductsModel>>(Arg.Any<string>()).Returns(products);
            var expectedProductsPagedResult = new PagedResult<GetProductsModel>(products.AsQueryable().Skip(getProductsQuery.PageSize * (getProductsQuery.Page - 1)).Take(getProductsQuery.PageSize).ToList(), products.AsQueryable().Skip(getProductsQuery.PageSize * (getProductsQuery.Page - 1)).Take(getProductsQuery.PageSize).ToList().Count);

            //Act
            var result = await getProductsQueryHandler.Handle(getProductsQuery, CancellationToken.None);

            //Assert
            await httpServiceMock.Received().GetAsync<IEnumerable<GetProductsModel>>(getProductsUrl);
            Assert.NotNull(result);
            Assert.Equal(expectedProductsPagedResult.Result.First().Id, result.Result.First().Id);
            Assert.Equal(expectedProductsPagedResult.Result.First().Name, result.Result.First().Name);
            Assert.Equal(expectedProductsPagedResult.Result.First().Data, result.Result.First().Data);
            Assert.Equal(expectedProductsPagedResult.Result.Last().Id, result.Result.Last().Id);
            Assert.Equal(expectedProductsPagedResult.Result.Last().Name, result.Result.Last().Name);
            Assert.Equal(expectedProductsPagedResult.Result.Last().Data, result.Result.Last().Data);
            Assert.Equal(expectedProductsPagedResult.Count, result.Count);
            Assert.Equivalent(expectedProductsPagedResult.Result, result.Result);
        }

        [Fact]
        public async Task FilterQuery()
        {
            //Arrange
            var getProductsUrl = "https://api.restful-api.dev/objects";
            var getProductsQuery = new GetProductsQuery("Apple", 5, 2);
            IEnumerable<GetProductsModel> products = new List<GetProductsModel>()
            {
                new() { Id = "1", Name = "Google Pixel 6 Pro", Data = new Data() { Color = "Cloudy White", Capacity = "128 GB" } },
                new() { Id = "2", Name = "Apple iPhone 12 Mini, 256GB, Blue" },
                new() { Id = "3", Name = "Apple iPhone 12 Pro Max", Data = new Data() { Color = "Cloudy White", CapacityGB = 512 } },
                new() { Id = "4", Name = "Apple iPhone 11, 64GB", Data = new Data() { Color = "Purple", Price = 389.99 } },
                new() { Id = "5", Name = "Samsung Galaxy Z Fold2", Data = new Data() { Color = "Brown", Price = 689.99 } },
                new() { Id = "6", Name = "Apple AirPods", Data = new Data() { Generation = "3rd", Price = 120 } },
                new() { Id = "7", Name = "Apple MacBook Pro 16", Data = new Data() { CPUModel = "Intel Core i9", HardDiskSize = "1 TB", Price = 1849.99, Year = 2019 } },
                new() { Id = "8", Name = "Apple Watch Series 8", Data = new Data() { CaseSize = "41mm", StrapColour = "Elderberry" } },
                new() { Id = "9", Name = "Beats Studio3 Wireless", Data = new Data() { Color = "Red", Description = "High-performance wireless noise cancelling headphones" } },
                new() { Id = "10", Name = "Apple iPad Mini 5th Gen", Data = new Data() { Capacity = "64 GB", ScreenSize = 7.9 } },
                new() { Id = "11", Name = "Apple iPad Mini 5th Gen", Data = new Data() { Capacity = "254 GB", ScreenSize = 7.9 } },
                new() { Id = "12", Name = "Apple iPad Air", Data = new Data() { Capacity = "64 GB", Generation = "4th", Price = 419.99 } },
                new() { Id = "13", Name = "Apple iPad Air", Data = new Data() { Capacity = "256 GB", Generation = "4th", Price = 519.99 } }
            };
            httpServiceMock.GetAsync<IEnumerable<GetProductsModel>>(Arg.Any<string>()).Returns(products);
            var expectedFilteredProducts = products.AsQueryable()
                .Where(product => (string.IsNullOrEmpty(getProductsQuery.Name) || product.Name.Contains(getProductsQuery.Name))
                ).Select(x => new GetProductsModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Data = x.Data
                });
            var resultList = expectedFilteredProducts.Skip(getProductsQuery.PageSize * (getProductsQuery.Page - 1)).Take(getProductsQuery.PageSize).ToList();
            var count = resultList.Count;
            var expectedFilteredProductsPagedResult = new PagedResult<GetProductsModel>(resultList, count);

            //Act
            var result = await getProductsQueryHandler.Handle(getProductsQuery, CancellationToken.None);

            //Assert
            await httpServiceMock.Received().GetAsync<IEnumerable<GetProductsModel>>(getProductsUrl);
            Assert.NotNull(result);
            Assert.Equal(expectedFilteredProductsPagedResult.Result.First().Id, result.Result.First().Id);
            Assert.Equal(expectedFilteredProductsPagedResult.Result.First().Name, result.Result.First().Name);
            Assert.Equal(expectedFilteredProductsPagedResult.Result.First().Data, result.Result.First().Data);
            Assert.Equal(expectedFilteredProductsPagedResult.Result.Last().Id, result.Result.Last().Id);
            Assert.Equal(expectedFilteredProductsPagedResult.Result.Last().Name, result.Result.Last().Name);
            Assert.Equal(expectedFilteredProductsPagedResult.Result.Last().Data, result.Result.Last().Data);
            Assert.Equal(expectedFilteredProductsPagedResult.Count, result.Count);
            Assert.Equivalent(expectedFilteredProductsPagedResult.Result, result.Result);
        }

        [Fact]
        public async Task EmptyQuery()
        {
            //Arrange
            var getProductsUrl = "https://api.restful-api.dev/objects";
            var getProductsQuery = new GetProductsQuery(null, 0, 0);
            IEnumerable<GetProductsModel> products = new List<GetProductsModel>()
            {
                new() { Id = "1", Name = "Google Pixel 6 Pro", Data = new Data() { Color = "Cloudy White", Capacity = "128 GB" } },
                new() { Id = "2", Name = "Apple iPhone 12 Mini, 256GB, Blue" },
                new() { Id = "3", Name = "Apple iPhone 12 Pro Max", Data = new Data() { Color = "Cloudy White", CapacityGB = 512 } },
                new() { Id = "4", Name = "Apple iPhone 11, 64GB", Data = new Data() { Color = "Purple", Price = 389.99 } },
                new() { Id = "5", Name = "Samsung Galaxy Z Fold2", Data = new Data() { Color = "Brown", Price = 689.99 } },
                new() { Id = "6", Name = "Apple AirPods", Data = new Data() { Generation = "3rd", Price = 120 } },
                new() { Id = "7", Name = "Apple MacBook Pro 16", Data = new Data() { CPUModel = "Intel Core i9", HardDiskSize = "1 TB", Price = 1849.99, Year = 2019 } },
                new() { Id = "8", Name = "Apple Watch Series 8", Data = new Data() { CaseSize = "41mm", StrapColour = "Elderberry" } },
                new() { Id = "9", Name = "Beats Studio3 Wireless", Data = new Data() { Color = "Red", Description = "High-performance wireless noise cancelling headphones" } },
                new() { Id = "10", Name = "Apple iPad Mini 5th Gen", Data = new Data() { Capacity = "64 GB", ScreenSize = 7.9 } },
                new() { Id = "11", Name = "Apple iPad Mini 5th Gen", Data = new Data() { Capacity = "254 GB", ScreenSize = 7.9 } },
                new() { Id = "12", Name = "Apple iPad Air", Data = new Data() { Capacity = "64 GB", Generation = "4th", Price = 419.99 } },
                new() { Id = "13", Name = "Apple iPad Air", Data = new Data() { Capacity = "256 GB", Generation = "4th", Price = 519.99 } }
            };
            httpServiceMock.GetAsync<IEnumerable<GetProductsModel>>(Arg.Any<string>()).Returns(products);
            var expectedEmptyProductsPagedResult = new PagedResult<GetProductsModel>([], 0);

            //Act
            var result = await getProductsQueryHandler.Handle(getProductsQuery, CancellationToken.None);

            //Assert
            await httpServiceMock.Received().GetAsync<IEnumerable<GetProductsModel>>(getProductsUrl);
            Assert.Empty(result.Result);
            Assert.Equal(0, result.Count);
            Assert.Equal(expectedEmptyProductsPagedResult.Result, result.Result);
            Assert.Equal(expectedEmptyProductsPagedResult.Count, result.Count);
        }
    }
}
