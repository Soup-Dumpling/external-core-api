using External.Product.Core.Services;
using External.Product.Core.UseCases.Product.GetProducts;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace External.Product.Core.UseCases.Product.GetProductsByIds
{
    public class GetProductsByIdsQueryHandler : IRequestHandler<GetProductsByIdsQuery, IEnumerable<GetProductsModel>>
    {
        private readonly IHttpService httpService;

        public GetProductsByIdsQueryHandler(IHttpService httpService) 
        {
            this.httpService = httpService;
        }

        public async Task<IEnumerable<GetProductsModel>> Handle(GetProductsByIdsQuery request, CancellationToken cancellationToken) 
        {
            var getProductsByIdsUrl = "https://api.restful-api.dev/objects?";

            for (int i = 0; i < request.Ids.Length; i++)
            {
                if (i == request.Ids.Length - 1)
                {
                    getProductsByIdsUrl += $"id={request.Ids[i]}";
                }
                else 
                {
                    getProductsByIdsUrl += $"id={request.Ids[i]}&";
                }
            }

            var products = await httpService.GetAsync<IEnumerable<GetProductsModel>>(getProductsByIdsUrl);
            return products;
        }
    }
}
