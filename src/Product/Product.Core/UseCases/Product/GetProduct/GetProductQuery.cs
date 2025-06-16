using External.Product.Core.UseCases.Product.GetProducts;
using MediatR;

namespace External.Product.Core.UseCases.Product.GetProduct
{
    public class GetProductQuery : IRequest<GetProductsModel>
    {
        public string Id { get; set; }

        public GetProductQuery(string id)
        {
            Id = id;
        }
    }
}
