using External.Product.Api.Models.Product;
using External.Product.Core.Models;
using External.Product.Core.UseCases.Product.AddProduct;
using External.Product.Core.UseCases.Product.DeleteProduct;
using External.Product.Core.UseCases.Product.GetProduct;
using External.Product.Core.UseCases.Product.GetProducts;
using External.Product.Core.UseCases.Product.GetProductsByIds;
using External.Product.Core.UseCases.Product.UpdateProduct;
using External.Product.Core.UseCases.Product.UpdateProductData;
using External.Product.Core.UseCases.Product.UpdateProductName;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace External.Product.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> logger;
        private readonly IMediator mediator;

        public ProductsController(ILogger<ProductsController> logger, IMediator mediator)
        {
            this.logger = logger;
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet]
        public async Task<PagedResult<GetProductsModel>> Get([FromQuery] GetProductsRequest request)
        {
            var query = new GetProductsQuery(request.Name, request.PageSize, request.Page);
            var response = await mediator.Send(query);
            return response;
        }

        [HttpGet("objects")]
        public async Task<IEnumerable<GetProductsModel>> Get([FromQuery] GetProductsByIdsRequest request)
        {
            var query = new GetProductsByIdsQuery(request.Ids);
            var response = await mediator.Send(query);
            return response;
        }

        [HttpGet("{id}")]
        public async Task<GetProductsModel> Get(string id)
        {
            var request = new GetProductQuery(id);
            var response = await mediator.Send(request);
            return response;
        }

        [HttpPost]
        public async Task<AddedProductModel> Post([FromBody] AddProductRequest model)
        {
            var request = new AddProductCommand(model.Name, model.Data);
            var response = await mediator.Send(request);
            return response;
        }

        [HttpPut("{id}")]
        public async Task<UpdatedProductModel> Put(string id, [FromBody] UpdateProductRequest model)
        {
            var request = new UpdateProductCommand(id, model.Name, model.Data);
            var response = await mediator.Send(request);
            return response;
        }

        [HttpPatch("data/{id}")]
        public async Task<UpdatedProductModel> PatchData(string id, [FromBody] UpdateProductDataRequest model)
        {
            var request = new UpdateProductDataCommand(id, model.Data);
            var response = await mediator.Send(request);
            return response;
        }

        [HttpPatch("name/{id}")]
        public async Task<UpdatedProductModel> PatchName(string id, [FromBody] UpdateProductNameRequest model)
        {
            var request = new UpdateProductNameCommand(id, model.Name);
            var response = await mediator.Send(request);
            return response;
        }

        [HttpDelete("{id}")]
        public async Task<DeletedProductModel> Delete(string id) 
        {
            var request = new DeleteProductCommand(id);
            var response = await mediator.Send(request);
            return response;
        }
    }
}
