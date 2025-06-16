using External.Product.Core.UseCases.Product.GetProducts;
using MediatR;

namespace External.Product.Core.UseCases.Product.AddProduct
{
    public class AddProductCommand : IRequest<AddedProductModel>
    {
        public string Name { get; set; }
        public Data Data { get; set; }

        public AddProductCommand(string name, Data data) 
        {
            Name = name;
            Data = data;
        }
    }
}
