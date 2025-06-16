using External.Product.Core.UseCases.Product.UpdateProductName;
using FluentValidation.TestHelper;
using System.Threading.Tasks;
using Xunit;

namespace External.Product.Core.UnitTests.UseCases.Product
{
    public class UpdateProductNameCommandValidatorTests
    {
        private readonly UpdateProductNameCommandValidator validator;

        public UpdateProductNameCommandValidatorTests() 
        {
            validator = new UpdateProductNameCommandValidator();
        }

        [Fact]
        public async Task ValidCommand()
        {
            var command = new UpdateProductNameCommand("ff80818196f2a23f019752339d694347", "Galaxy S25 Ultra Pro");
            var result = await validator.TestValidateAsync(command);
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public async Task InvalidCommand()
        {
            var command = new UpdateProductNameCommand(string.Empty, string.Empty);
            var result = await validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(x => x.Id);
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }
    }
}
