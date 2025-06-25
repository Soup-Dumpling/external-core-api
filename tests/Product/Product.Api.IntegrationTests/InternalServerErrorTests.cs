using Alba;
using External.Product.Api.Models.Product;
using External.Product.Core.UseCases.Product.GetProducts;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace External.Product.Api.IntegrationTests
{
    [Collection("Integration")]
    public class InternalServerErrorTests
    {
        private readonly IAlbaHost host;

        public InternalServerErrorTests(AppFixture fixture) 
        {
            host = fixture.Host;
        }

        [Theory]
        [InlineData("/api/products/3")]
        public async Task PutReturnsInternalServerError(string url)
        {
            var response = await host.Scenario(_ =>
            {
                _.Put.Json<UpdateProductRequest>(new UpdateProductRequest() { Name = "Apple iPhone 14 Pro Max", Data = new Data { CapacityGB = 512, Color = "Coral Blue", Year = 2022 } }).ToUrl(url);
                _.StatusCodeShouldBe(HttpStatusCode.InternalServerError);
            });
        }

        [Theory]
        [InlineData("/api/products/data/3")]
        public async Task PatchProductDataReturnsInternalServerError(string url)
        {
            var response = await host.Scenario(_ =>
            {
                _.Patch.Json<UpdateProductDataRequest>(new UpdateProductDataRequest() { Data = new Data { CapacityGB = 512, Color = "Coral Blue", Year = 2022 } }).ToUrl(url);
                _.StatusCodeShouldBe(HttpStatusCode.InternalServerError);
            });
        }

        [Theory]
        [InlineData("/api/products/name/3")]
        public async Task PatchProductNameReturnsInternalServerError(string url)
        {
            var response = await host.Scenario(_ =>
            {
                _.Patch.Json<UpdateProductNameRequest>(new UpdateProductNameRequest() { Name = "Updated Product Name" }).ToUrl(url);
                _.StatusCodeShouldBe(HttpStatusCode.InternalServerError);
            });
        }

        [Theory]
        [InlineData("/api/products/3")]
        public async Task DeleteReturnsInternalServerError(string url)
        {
            var response = await host.Scenario(_ =>
            {
                _.Delete.Url(url);
                _.StatusCodeShouldBe(HttpStatusCode.InternalServerError);
            });
        }
    }
}
