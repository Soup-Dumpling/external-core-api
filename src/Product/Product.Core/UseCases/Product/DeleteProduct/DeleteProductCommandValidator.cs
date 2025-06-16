using FluentValidation;

namespace External.Product.Core.UseCases.Product.DeleteProduct
{
    public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
    {
        public DeleteProductCommandValidator() 
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}
