using FluentValidation;

namespace External.Product.Core.UseCases.Product.GetProducts
{
    public class GetProductsQueryValidator : AbstractValidator<GetProductsQuery>
    {
        public GetProductsQueryValidator() 
        {
            RuleFor(x => x.PageSize).GreaterThan(0);
        }
    }
}
