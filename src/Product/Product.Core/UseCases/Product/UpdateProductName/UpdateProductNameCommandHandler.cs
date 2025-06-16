using External.Product.Core.Services;
using External.Product.Core.UseCases.Product.UpdateProduct;
using MediatR;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace External.Product.Core.UseCases.Product.UpdateProductName
{
    public class UpdateProductNameCommandHandler : IRequestHandler<UpdateProductNameCommand, UpdatedProductModel>
    {
        private readonly IHttpService httpService;

        public UpdateProductNameCommandHandler(IHttpService httpService) 
        {
            this.httpService = httpService;
        }

        public async Task<UpdatedProductModel> Handle(UpdateProductNameCommand request, CancellationToken cancellationToken)
        {
            var updateProductNameUrl = $"https://api.restful-api.dev/objects/{request.Id}";
            StringContent content = new(JsonConvert.SerializeObject(new { name = request.Name }), Encoding.UTF8, "application/json");
            var updatedProduct = await httpService.PatchAsync<UpdatedProductModel>(updateProductNameUrl, content);
            return updatedProduct;
        }
    }
}
