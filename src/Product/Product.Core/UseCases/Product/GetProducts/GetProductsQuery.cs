using External.Product.Core.Models;
using MediatR;

namespace External.Product.Core.UseCases.Product.GetProducts
{
    public class GetProductsQuery : IRequest<PagedResult<GetProductsModel>>
    {
        public string Name { get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }
        public GetProductsQuery(string name, int pageSize, int page) 
        {
            Name = name;
            PageSize = pageSize;
            Page = page;
        }
    }
}
