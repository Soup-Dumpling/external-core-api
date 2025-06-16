using MediatR;

namespace External.Product.Core.UseCases.Product.DeleteProduct
{
    public class DeleteProductCommand : IRequest<DeletedProductModel>
    {
        public string Id { get; set; }

        public DeleteProductCommand(string id) 
        {
            Id = id;
        }
    }
}
