using Microsoft.AspNetCore.Mvc;

namespace External.Product.Api.Models.Product
{
    public class GetProductsByIdsRequest
    {
        [FromQuery(Name = "id")]
        public string[] Ids { get; set; }
    }
}
