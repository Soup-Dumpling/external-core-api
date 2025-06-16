using External.Product.Core.UseCases.Product.GetProducts;
using MediatR;

namespace External.Product.Core.UseCases.Product.UpdateProduct
{
    public class UpdateProductCommand : IRequest<UpdatedProductModel>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Data Data { get; set; }

        public UpdateProductCommand(string id, string name, Data data) 
        {
            Id = id;
            Name = name;
            Data = data;
        }
    }
}
