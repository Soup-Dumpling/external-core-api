using External.Product.Core.UseCases.Product.GetProducts;
using System;

namespace External.Product.Core.UseCases.Product.UpdateProduct
{
    public class UpdatedProductModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Data Data { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
