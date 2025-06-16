using FluentValidation;

namespace External.Product.Core.UseCases.Product.UpdateProductName
{
    public class UpdateProductNameCommandValidator : AbstractValidator<UpdateProductNameCommand>
    {
        public UpdateProductNameCommandValidator() 
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}
