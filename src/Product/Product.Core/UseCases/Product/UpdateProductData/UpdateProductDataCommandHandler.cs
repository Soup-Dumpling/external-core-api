using External.Product.Core.Services;
using External.Product.Core.UseCases.Product.UpdateProduct;
using MediatR;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace External.Product.Core.UseCases.Product.UpdateProductData
{
    public class UpdateProductDataCommandHandler : IRequestHandler<UpdateProductDataCommand, UpdatedProductModel>
    {
        private readonly IHttpService httpService;

        public UpdateProductDataCommandHandler(IHttpService httpService) 
        {
            this.httpService = httpService;
        }

        public async Task<UpdatedProductModel> Handle(UpdateProductDataCommand request, CancellationToken cancellationToken) 
        {
            var updateProductDataUrl = $"https://api.restful-api.dev/objects/{request.Id}";
            StringContent content = new(JsonConvert.SerializeObject(new { data = request.Data }), Encoding.UTF8, "application/json");
            var updatedProduct = await httpService.PatchAsync<UpdatedProductModel>(updateProductDataUrl, content);
            return updatedProduct;
        }
    }
}
