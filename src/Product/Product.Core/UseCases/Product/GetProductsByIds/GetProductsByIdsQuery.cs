using External.Product.Core.UseCases.Product.GetProducts;
using MediatR;
using System.Collections.Generic;

namespace External.Product.Core.UseCases.Product.GetProductsByIds
{
    public class GetProductsByIdsQuery : IRequest<IEnumerable<GetProductsModel>>
    {
        public string[] Ids {  get; set; }

        public GetProductsByIdsQuery(string[] ids) 
        {
            Ids = ids;
        }
    }
}
