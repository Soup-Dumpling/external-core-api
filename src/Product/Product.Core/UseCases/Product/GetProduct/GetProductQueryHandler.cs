using External.Product.Core.Services;
using External.Product.Core.UseCases.Product.GetProducts;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace External.Product.Core.UseCases.Product.GetProduct
{
    public class GetProductQueryHandler : IRequestHandler<GetProductQuery, GetProductsModel>
    {
        private readonly IHttpService httpService;

        public GetProductQueryHandler(IHttpService httpService) 
        {
            this.httpService = httpService;
        }

        public async Task<GetProductsModel> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            var getProductUrl = $"https://api.restful-api.dev/objects/{request.Id}";
            var product = await httpService.GetAsync<GetProductsModel>(getProductUrl);
            return product;
        }
    }
}
