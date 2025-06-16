using External.Product.Core.Services;
using MediatR;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace External.Product.Core.UseCases.Product.AddProduct
{
    public class AddProductCommandHandler : IRequestHandler<AddProductCommand, AddedProductModel>
    {
        private readonly IHttpService httpService;

        public AddProductCommandHandler(IHttpService httpService)
        {
            this.httpService = httpService;
        }

        public async Task <AddedProductModel> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            var addProductUrl = "https://api.restful-api.dev/objects";
            StringContent content = new(JsonConvert.SerializeObject(new { name = request.Name, data = request.Data}), Encoding.UTF8, "application/json");
            var addedProduct = await httpService.PostAsync<AddedProductModel>(addProductUrl, content);
            return addedProduct;
        }
    }
}
