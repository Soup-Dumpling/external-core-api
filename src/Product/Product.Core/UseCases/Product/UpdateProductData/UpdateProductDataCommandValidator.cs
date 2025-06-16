using FluentValidation;

namespace External.Product.Core.UseCases.Product.UpdateProductData
{
    public class UpdateProductDataCommandValidator : AbstractValidator<UpdateProductDataCommand>
    {
        public UpdateProductDataCommandValidator() 
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Data).NotEmpty();
        }
    }
}
