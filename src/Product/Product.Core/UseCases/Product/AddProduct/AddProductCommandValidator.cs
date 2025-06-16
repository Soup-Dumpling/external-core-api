using FluentValidation;

namespace External.Product.Core.UseCases.Product.AddProduct
{
    public class AddProductCommandValidator : AbstractValidator<AddProductCommand>
    {
        public AddProductCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Data).NotEmpty();
        }
    }
}
