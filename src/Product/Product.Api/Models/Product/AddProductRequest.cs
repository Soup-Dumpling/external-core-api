using External.Product.Core.UseCases.Product.GetProducts;

namespace External.Product.Api.Models.Product
{
    public class AddProductRequest
    {
        public string Name { get; set; }
        public Data Data { get; set; }
    }
}
