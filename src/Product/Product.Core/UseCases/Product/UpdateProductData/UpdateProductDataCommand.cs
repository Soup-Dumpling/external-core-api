using External.Product.Core.UseCases.Product.GetProducts;
using External.Product.Core.UseCases.Product.UpdateProduct;
using MediatR;

namespace External.Product.Core.UseCases.Product.UpdateProductData
{
    public class UpdateProductDataCommand : IRequest<UpdatedProductModel>
    {
        public string Id { get; set; }
        public Data Data { get; set; }

        public UpdateProductDataCommand(string id, Data data) 
        {
            Id = id;
            Data = data;
        }
    }
}
