using FluentValidation;

namespace External.Product.Core.UseCases.Product.GetProduct
{
    public class GetProductQueryValidator : AbstractValidator<GetProductQuery>
    {
        public GetProductQueryValidator() 
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}
