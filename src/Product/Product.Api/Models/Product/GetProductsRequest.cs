using Microsoft.AspNetCore.Mvc;

namespace External.Product.Api.Models.Product
{
    public class GetProductsRequest
    {
        [FromQuery(Name = "name")]
        public string Name { get; set; }
        [FromQuery(Name = "pageSize")]
        public int PageSize { get; set; } = 10;
        [FromQuery(Name = "page")]
        public int Page { get; set; } = 1;
    }
}
