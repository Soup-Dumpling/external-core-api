using External.Product.Core.Services;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace External.Product.Core.UseCases.Product.DeleteProduct
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, DeletedProductModel>
    {
        private readonly IHttpService httpService;

        public DeleteProductCommandHandler(IHttpService httpService) 
        {
            this.httpService = httpService;
        }

        public async Task <DeletedProductModel> Handle(DeleteProductCommand request, CancellationToken cancellationToken) 
        {
            var deleteProductUrl = $"https://api.restful-api.dev/objects/{request.Id}";
            var deletedProduct = await httpService.DeleteAsync<DeletedProductModel>(deleteProductUrl);
            return deletedProduct;
        }
    }
}
