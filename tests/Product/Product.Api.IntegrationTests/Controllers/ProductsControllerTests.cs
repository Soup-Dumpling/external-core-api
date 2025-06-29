﻿using Alba;
using External.Product.Api.Models.Product;
using External.Product.Core.Models;
using External.Product.Core.UseCases.Product.AddProduct;
using External.Product.Core.UseCases.Product.DeleteProduct;
using External.Product.Core.UseCases.Product.GetProducts;
using External.Product.Core.UseCases.Product.UpdateProduct;
using FizzWare.NBuilder;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace External.Product.Api.IntegrationTests.Controllers
{
    [Collection("Integration")]
    public class ProductsControllerTests
    {
        private readonly IAlbaHost host;

        public ProductsControllerTests(AppFixture fixture)
        {
            host = fixture.Host;
        }

        public static IEnumerable<object[]> FilterProducts
        {
            get
            {
                yield return new object[] { new GetProductsRequest() { }, /*expectedCount*/ 10, /*verifyData*/ true };
                yield return new object[] { new GetProductsRequest() { PageSize = 5 }, /*expectedCount*/ 5, /*verifyData*/ false };
                yield return new object[] { new GetProductsRequest() { Name = "Google Pixel 6 Pro" }, /*expectedCount*/ 1, /*verifyData*/ false };
            }
        }

        [Theory]
        [MemberData(nameof(FilterProducts))]
        public async Task GetProducts(GetProductsRequest request, int expectedCount, bool verifyData)
        {
            //Arrange
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
                new() { Id = "10", Name = "Apple iPad Mini 5th Gen", Data = new Data() { Capacity = "64 GB", ScreenSize = 7.9 } }
            };

            //Act
            var response = await host.Scenario(_ =>
            {
                _.Get.Url($"/api/products?name={request.Name}&pageSize={request.PageSize}&page={request.Page}");
                _.StatusCodeShouldBeOk();
            });

            //Assert
            var result = response.ReadAsJson<PagedResult<GetProductsModel>>();
            Assert.Equal(expectedCount, result.Result.Count());
            if (verifyData)
            {
                var expectedList = products.Select(x => new GetProductsModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Data = x.Data
                }).ToList();
                result.Result.Should().BeEquivalentTo(expectedList);
                result.Result.First().Equals(expectedList[0]);
                result.Result.Last().Equals(expectedList[9]);
            }
        }

        [Fact]
        public async Task GetProductsByIds()
        {
            //Arrange
            IEnumerable<GetProductsModel> expectedProducts = new List<GetProductsModel>()
            {
                new () { Id = "3", Name = "Apple iPhone 12 Pro Max", Data = new Data { CapacityGB = 512, Color = "Cloudy White" } },
                new () { Id = "5", Name = "Samsung Galaxy Z Fold2", Data = new Data { Color = "Brown", Price = 689.99 } },
                new () { Id = "10", Name = "Apple iPad Mini 5th Gen", Data = new Data { Capacity = "64 GB", ScreenSize = 7.9 } }
            };

            //Act
            var response = await host.Scenario(_ =>
            {
                _.Get.Url($"/api/products/objects?id=3&id=5&id=10");
                _.StatusCodeShouldBeOk();
            });

            //Assert
            var result = response.ReadAsJson<IEnumerable<GetProductsModel>>();
            Assert.Equal(3, result.Count());
            result.Should().BeEquivalentTo(expectedProducts);
        }

        [Fact]
        public async Task InvalidGetProductsByIds()
        {
            //Arrange
            var request = Builder<GetProductsByIdsRequest>.CreateNew()
                .Do(x =>
                {
                    x.Ids = ["3", "", "10"];
                }).Build();

            //Act And Assert
            await host.Scenario(_ =>
            {
                _.Get.Url($"/api/products/objects?id=3&id=&id=10");
                _.StatusCodeShouldBe(HttpStatusCode.BadRequest);
                _.ContentShouldContain("The request contains invalid parameters.");
            });
        }

        [Fact]
        public async Task GetProductById()
        {
            //Arrange
            var expectedProduct = new GetProductsModel()
            {
                Id = "7",
                Name = "Apple MacBook Pro 16",
                Data = new Data { CPUModel = "Intel Core i9", HardDiskSize = "1 TB", Price = 1849.99, Year = 2019 }
            };

            //Act
            var response = await host.Scenario(_ =>
            {
                _.Get.Url($"/api/products/{expectedProduct.Id}");
                _.StatusCodeShouldBeOk();
            });

            //Assert
            var result = response.ReadAsJson<GetProductsModel>();
            Assert.Equal(expectedProduct.Id, result.Id);
            Assert.Equal(expectedProduct.Name, result.Name);
            result.Data.Should().BeEquivalentTo(expectedProduct.Data);
        }

        [Fact]
        public async Task AddProduct()
        {
            //Arrange
            var request = Builder<AddProductRequest>.CreateNew()
                .Do(x =>
                {
                    x.Name = "Galaxy S25 Ultra";
                    x.Data = new Data { Capacity = "512 GB", Color = "Titanium Silverblue", Price = 1349, Year = 2025 };
                }).Build();

            //Act
            var response = await host.Scenario(_ =>
            {
                _.Post.Json<AddProductRequest>(request).ToUrl("/api/products");
                _.StatusCodeShouldBeOk();
            });

            //Assert
            var result = response.ReadAsJson<AddedProductModel>();
            Assert.NotEqual(string.Empty, result.Id);
            var addedProductGetResponse = await host.Scenario(_ =>
            {
                _.Get.Url($"/api/products/{result.Id}");
                _.StatusCodeShouldBeOk();
            });
            var getAddedProduct = addedProductGetResponse.ReadAsJson<GetProductsModel>();
            var expectedAddedProduct = new AddedProductModel
            {
                Id = getAddedProduct.Id,
                Name = getAddedProduct.Name,
                Data = getAddedProduct.Data,
                CreatedAt = DateTime.Now,
            };
            result.Should().BeEquivalentTo(expectedAddedProduct,
                options =>
                {
                    options.Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, TimeSpan.FromMinutes(1))).WhenTypeIs<DateTime>();
                    return options;
                });
        }

        [Fact]
        public async Task UpdateProduct()
        {
            //Arrange
            var addProductRequest = Builder<AddProductRequest>.CreateNew()
                .Do(x =>
                {
                    x.Name = "Galaxy S25 Ultra";
                    x.Data = new Data { Capacity = "512 GB", Color = "Titanium Silverblue", Price = 1349, Year = 2025 };
                }).Build();

            var addedProductResponse = await host.Scenario(_ =>
            {
                _.Post.Json<AddProductRequest>(addProductRequest).ToUrl("/api/products");
                _.StatusCodeShouldBeOk();
            });

            var addedProductResult = addedProductResponse.ReadAsJson<AddedProductModel>();

            var request = Builder<UpdateProductRequest>.CreateNew()
                .Do(x =>
                {
                    x.Name = "Galaxy S25 Ultra Pro";
                    x.Data = new Data { Capacity = "1 TB", Color = "Titanium Silverblue", Price = 1649, Year = 2025 };
                }).Build();

            //Act
            var response = await host.Scenario(_ =>
            {
                _.Put.Json<UpdateProductRequest>(request).ToUrl($"/api/products/{addedProductResult.Id}");
                _.StatusCodeShouldBeOk();
            });

            //Assert
            var result = response.ReadAsJson<UpdatedProductModel>();
            Assert.Equal(addedProductResult.Id, result.Id);
            Assert.Equal(request.Name, result.Name);
            var expectedProductData = new Data { Capacity = request.Data.Capacity, Color = request.Data.Color, Price = request.Data.Price, Year = request.Data.Year };
            result.Data.Should().BeEquivalentTo(expectedProductData);
            var updatedProductGetResponse = await host.Scenario(_ =>
            {
                _.Get.Url($"/api/products/{result.Id}");
                _.StatusCodeShouldBeOk();
            });
            var getUpdatedProduct = updatedProductGetResponse.ReadAsJson<GetProductsModel>();
            var expectedUpdatedProduct = new UpdatedProductModel
            {
                Id = getUpdatedProduct.Id,
                Name = getUpdatedProduct.Name,
                Data = getUpdatedProduct.Data,
                UpdatedAt = DateTime.Now,
            };
            result.Should().BeEquivalentTo(expectedUpdatedProduct,
                options =>
                {
                    options.Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, TimeSpan.FromMinutes(1))).WhenTypeIs<DateTime>();
                    return options;
                });
        }

        [Fact]
        public async Task UpdateProductData()
        {
            //Arrange
            var addProductRequest = Builder<AddProductRequest>.CreateNew()
                .Do(x =>
                {
                    x.Name = "Galaxy S25 Ultra";
                    x.Data = new Data { Capacity = "512 GB", Color = "Titanium Silverblue", Price = 1349, Year = 2025 };
                }).Build();

            var addedProductResponse = await host.Scenario(_ =>
            {
                _.Post.Json<AddProductRequest>(addProductRequest).ToUrl("/api/products");
                _.StatusCodeShouldBeOk();
            });

            var addedProductResult = addedProductResponse.ReadAsJson<AddedProductModel>();

            var request = Builder<UpdateProductDataRequest>.CreateNew()
                .Do(x =>
                {
                    x.Data = new Data { Capacity = "1 TB", Color = "Titanium Silverblue", Price = 1649, Year = 2025 };
                }).Build();

            //Act
            var response = await host.Scenario(_ =>
            {
                _.Patch.Json<UpdateProductDataRequest>(request).ToUrl($"/api/products/data/{addedProductResult.Id}");
                _.StatusCodeShouldBeOk();
            });

            //Assert
            var result = response.ReadAsJson<UpdatedProductModel>();
            Assert.Equal(addedProductResult.Id, result.Id);
            var expectedProductData = new Data { Capacity = request.Data.Capacity, Color = request.Data.Color, Price = request.Data.Price, Year = request.Data.Year };
            result.Data.Should().BeEquivalentTo(expectedProductData);
            var updatedProductGetResponse = await host.Scenario(_ =>
            {
                _.Get.Url($"/api/products/{result.Id}");
                _.StatusCodeShouldBeOk();
            });
            var getUpdatedProduct = updatedProductGetResponse.ReadAsJson<GetProductsModel>();
            var expectedUpdatedProduct = new UpdatedProductModel
            {
                Id = getUpdatedProduct.Id,
                Name = getUpdatedProduct.Name,
                Data = getUpdatedProduct.Data,
                UpdatedAt = DateTime.Now,
            };
            result.Should().BeEquivalentTo(expectedUpdatedProduct,
                options =>
                {
                    options.Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, TimeSpan.FromMinutes(1))).WhenTypeIs<DateTime>();
                    return options;
                });
        }

        [Fact]
        public async Task UpdateProductName()
        {
            //Arrange
            var addProductRequest = Builder<AddProductRequest>.CreateNew()
                .Do(x =>
                {
                    x.Name = "Galaxy S25 Ultra";
                    x.Data = new Data { Capacity = "512 GB", Color = "Titanium Silverblue", Price = 1349, Year = 2025 };
                }).Build();

            var addedProductResponse = await host.Scenario(_ =>
            {
                _.Post.Json<AddProductRequest>(addProductRequest).ToUrl("/api/products");
                _.StatusCodeShouldBeOk();
            });

            var addedProductResult = addedProductResponse.ReadAsJson<AddedProductModel>();

            var request = Builder<UpdateProductNameRequest>.CreateNew()
                .Do(x =>
                {
                    x.Name = "Galaxy S25 Ultra Pro";
                }).Build();

            //Act
            var response = await host.Scenario(_ =>
            {
                _.Patch.Json<UpdateProductNameRequest>(request).ToUrl($"/api/products/name/{addedProductResult.Id}");
                _.StatusCodeShouldBeOk();
            });

            //Assert
            var result = response.ReadAsJson<UpdatedProductModel>();
            Assert.Equal(addedProductResult.Id, result.Id);
            Assert.Equal(request.Name, result.Name);
            var updatedProductGetResponse = await host.Scenario(_ =>
            {
                _.Get.Url($"/api/products/{result.Id}");
                _.StatusCodeShouldBeOk();
            });
            var getUpdatedProduct = updatedProductGetResponse.ReadAsJson<GetProductsModel>();
            var expectedUpdatedProduct = new UpdatedProductModel
            {
                Id = getUpdatedProduct.Id,
                Name = getUpdatedProduct.Name,
                Data = getUpdatedProduct.Data,
                UpdatedAt = DateTime.Now,
            };
            result.Should().BeEquivalentTo(expectedUpdatedProduct,
                options =>
                {
                    options.Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, TimeSpan.FromMinutes(1))).WhenTypeIs<DateTime>();
                    return options;
                });
        }

        [Fact]
        public async Task DeleteProduct()
        {
            //Arrange
            var addProductRequest = Builder<AddProductRequest>.CreateNew()
                .Do(x =>
                {
                    x.Name = "Galaxy S25 Ultra";
                    x.Data = new Data { Capacity = "512 GB", Color = "Titanium Silverblue", Price = 1349, Year = 2025 };
                }).Build();

            var addedProductResponse = await host.Scenario(_ =>
            {
                _.Post.Json<AddProductRequest>(addProductRequest).ToUrl("/api/products");
                _.StatusCodeShouldBeOk();
            });

            var addedProductResult = addedProductResponse.ReadAsJson<AddedProductModel>();

            var expectedDeleteResponse = $"Object with id = {addedProductResult.Id} has been deleted.";

            //Act
            var response = await host.Scenario(_ =>
            {
                _.Delete.Url($"/api/products/{addedProductResult.Id}");
                _.StatusCodeShouldBeOk();
            });

            //Assert
            var result = response.ReadAsJson<DeletedProductModel>();
            Assert.NotEqual(string.Empty, result.Message);
            Assert.Equal(expectedDeleteResponse, result.Message);
        }
    }
}
