using External.Product.Core.UseCases.Product.DeleteProduct;
using FluentValidation.TestHelper;
using System.Threading.Tasks;
using Xunit;

namespace External.Product.Core.UnitTests.UseCases.Product
{
    public class DeleteProductCommandValidatorTests
    {
        private readonly DeleteProductCommandValidator validator;

        public DeleteProductCommandValidatorTests() 
        {
            validator = new DeleteProductCommandValidator();
        }

        [Fact]
        public async Task ValidCommand()
        {
            var command = new DeleteProductCommand("ff80818196f2a23f019752339d694347");
            var result = await validator.TestValidateAsync(command);
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public async Task InvalidCommand()
        {
            var command = new DeleteProductCommand(string.Empty);
            var result = await validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(x => x.Id);
        }
    }
}
