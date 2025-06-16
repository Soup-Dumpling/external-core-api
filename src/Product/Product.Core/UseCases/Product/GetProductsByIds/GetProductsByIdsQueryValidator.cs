using FluentValidation;

namespace External.Product.Core.UseCases.Product.GetProductsByIds
{
    public class GetProductsByIdsQueryValidator : AbstractValidator<GetProductsByIdsQuery>
    {
        public GetProductsByIdsQueryValidator() 
        {
            RuleFor(x => x.Ids).NotEmpty();
        }
    }
}
