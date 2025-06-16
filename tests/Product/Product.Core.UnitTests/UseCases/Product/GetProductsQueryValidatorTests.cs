using External.Product.Core.UseCases.Product.GetProducts;
using FluentValidation.TestHelper;
using System.Threading.Tasks;
using Xunit;

namespace External.Product.Core.UnitTests.UseCases.Product
{
    public class GetProductsQueryValidatorTests
    {
        private readonly GetProductsQueryValidator validator;

        public GetProductsQueryValidatorTests() 
        {
            validator = new GetProductsQueryValidator();
        }

        [Fact]
        public async Task ValidQuery()
        {
            var query = new GetProductsQuery("productName", 10, 1);
            var result = await validator.TestValidateAsync(query);
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public async Task InvalidQuery()
        {
            var query = new GetProductsQuery("productName", 0, 1);
            var result = await validator.TestValidateAsync(query);
            result.ShouldHaveValidationErrorFor(x => x.PageSize);
        }
    }
}
