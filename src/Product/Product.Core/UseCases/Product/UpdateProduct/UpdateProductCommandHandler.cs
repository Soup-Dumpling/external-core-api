using External.Product.Core.Services;
using MediatR;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace External.Product.Core.UseCases.Product.UpdateProduct
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, UpdatedProductModel>
    {
        private readonly IHttpService httpService;

        public UpdateProductCommandHandler(IHttpService httpService) 
        {
            this.httpService = httpService;
        }

        public async Task<UpdatedProductModel> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var updateProductUrl = $"https://api.restful-api.dev/objects/{request.Id}";
            StringContent content = new(JsonConvert.SerializeObject(new { name = request.Name, data = request.Data }), Encoding.UTF8, "application/json");
            var updatedProduct = await httpService.PutAsync<UpdatedProductModel>(updateProductUrl, content);
            return updatedProduct;
        }
    }
}
