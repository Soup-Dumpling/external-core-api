using External.Product.Core.UseCases.Product.GetProduct;
using FluentValidation.TestHelper;
using System.Threading.Tasks;
using Xunit;

namespace External.Product.Core.UnitTests.UseCases.Product
{
    public class GetProductQueryValidatorTests
    {
        private readonly GetProductQueryValidator validator;

        public GetProductQueryValidatorTests() 
        {
            validator = new GetProductQueryValidator();
        }

        [Fact]
        public async Task ValidQuery()
        {
            var query = new GetProductQuery("1");
            var result = await validator.TestValidateAsync(query);
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public async Task InvalidQuery()
        {
            var query = new GetProductQuery(string.Empty);
            var result = await validator.TestValidateAsync(query);
            result.ShouldHaveValidationErrorFor(x => x.Id);
        }
    }
}
