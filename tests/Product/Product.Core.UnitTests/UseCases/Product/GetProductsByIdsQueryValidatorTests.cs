using External.Product.Core.UseCases.Product.GetProductsByIds;
using FluentValidation.TestHelper;
using System.Threading.Tasks;
using Xunit;

namespace External.Product.Core.UnitTests.UseCases.Product
{
    public class GetProductsByIdsQueryValidatorTests
    {
        private readonly GetProductsByIdsQueryValidator validator;

        public GetProductsByIdsQueryValidatorTests() 
        {
            validator = new GetProductsByIdsQueryValidator();
        }

        [Fact]
        public async Task ValidQuery()
        {
            var query = new GetProductsByIdsQuery(["1", "2", "3"]);
            var result = await validator.TestValidateAsync(query);
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public async Task InvalidQuery()
        {
            var query = new GetProductsByIdsQuery([]);
            var result = await validator.TestValidateAsync(query);
            result.ShouldHaveValidationErrorFor(x => x.Ids);
        }
    }
}
