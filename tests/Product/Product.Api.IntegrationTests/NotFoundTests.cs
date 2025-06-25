using Alba;
using External.Product.Api.Models.Product;
using External.Product.Core.UseCases.Product.GetProducts;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace External.Product.Api.IntegrationTests
{
    [Collection("Integration")]
    public class NotFoundTests
    {
        private readonly IAlbaHost host;

        public NotFoundTests(AppFixture fixture) 
        {
            host = fixture.Host;
        }

        [Theory]
        [InlineData("/api/products/h1k8jupkovwbnbqa24z7t3eyxk9i4alw")]
        public async Task GetReturnsNotFound(string url)
        {
            var response = await host.Scenario(_ =>
            {
                _.Get.Url(url);
                _.StatusCodeShouldBe(HttpStatusCode.NotFound);
            });
        }

        [Theory]
        [InlineData("/api/products/h1k8jupkovwbnbqa24z7t3eyxk9i4alw")]
        public async Task PutReturnsNotFound(string url)
        {
            var response = await host.Scenario(_ =>
            {
                _.Put.Json<UpdateProductRequest>(new UpdateProductRequest() { Name = "Updated Product Name", Data = new Data { Color = "Violet" } }).ToUrl(url);
                _.StatusCodeShouldBe(HttpStatusCode.NotFound);
            });
        }

        [Theory]
        [InlineData("/api/products/data/h1k8jupkovwbnbqa24z7t3eyxk9i4alw")]
        public async Task PatchProductDataReturnsNotFound(string url)
        {
            var response = await host.Scenario(_ =>
            {
                _.Patch.Json<UpdateProductDataRequest>(new UpdateProductDataRequest() { Data = new Data { Color = "Violet" } }).ToUrl(url);
                _.StatusCodeShouldBe(HttpStatusCode.NotFound);
            });
        }

        [Theory]
        [InlineData("/api/products/name/h1k8jupkovwbnbqa24z7t3eyxk9i4alw")]
        public async Task PatchProductNameReturnsNotFound(string url)
        {
            var response = await host.Scenario(_ =>
            {
                _.Patch.Json<UpdateProductNameRequest>(new UpdateProductNameRequest() { Name = "Updated Product Name" }).ToUrl(url);
                _.StatusCodeShouldBe(HttpStatusCode.NotFound);
            });
        }

        [Theory]
        [InlineData("/api/products/h1k8jupkovwbnbqa24z7t3eyxk9i4alw")]
        public async Task DeleteReturnsNotFound(string url)
        {
            var response = await host.Scenario(_ =>
            {
                _.Delete.Url(url);
                _.StatusCodeShouldBe(HttpStatusCode.NotFound);
            });
        }
    }
}
