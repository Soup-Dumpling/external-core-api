using External.Product.Core.Exceptions;
using External.Product.Core.Models;
using External.Product.Core.Services;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace External.Product.Core.UseCases.Product.GetProducts
{
    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, PagedResult<GetProductsModel>>
    {
        private readonly IHttpService httpService;

        public GetProductsQueryHandler(IHttpService httpService)
        {
            this.httpService = httpService;
        }

        public async Task<PagedResult<GetProductsModel>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var getProductsUrl = "https://api.restful-api.dev/objects";
            var products = await httpService.GetAsync<IEnumerable<GetProductsModel>>(getProductsUrl) ?? throw new NotFoundException();
            IQueryable<GetProductsModel> query = products.AsQueryable();
            query = query.
                Where(product => (string.IsNullOrEmpty(request.Name) || product.Name.Contains(request.Name))
                ).Select(x => new GetProductsModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Data = x.Data
                });

            if (request.PageSize > 0 && request.Page > 0)
            {
                var result = query.Skip(request.PageSize * (request.Page - 1)).Take(request.PageSize).ToList();
                var count = result.Count;
                return new PagedResult<GetProductsModel>(result, count);
            }
            else
            {
                return new PagedResult<GetProductsModel>([], 0);
            }
        }
    }
}
