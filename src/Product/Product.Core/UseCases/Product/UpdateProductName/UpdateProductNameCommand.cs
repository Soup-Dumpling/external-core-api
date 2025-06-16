using External.Product.Core.UseCases.Product.UpdateProduct;
using MediatR;

namespace External.Product.Core.UseCases.Product.UpdateProductName
{
    public class UpdateProductNameCommand : IRequest<UpdatedProductModel>
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public UpdateProductNameCommand(string id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
