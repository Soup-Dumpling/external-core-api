using External.Product.Core.UseCases.Product.GetProducts;
using System;

namespace External.Product.Core.UseCases.Product.AddProduct
{
    public class AddedProductModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Data Data { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
