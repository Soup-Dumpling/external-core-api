using External.Product.Core.UseCases.Product.GetProducts;
using External.Product.Core.UseCases.Product.UpdateProduct;
using FluentValidation.TestHelper;
using System.Threading.Tasks;
using Xunit;

namespace External.Product.Core.UnitTests.UseCases.Product
{
    public class UpdateProductCommandValidatorTests
    {
        private readonly UpdateProductCommandValidator validator;

        public UpdateProductCommandValidatorTests()
        {
            validator = new UpdateProductCommandValidator();
        }

        [Fact]
        public async Task ValidCommand()
        {
            var data = new Data { Capacity = "1 TB", Color = "Titanium Silverblue", Price = 1649, Year = 2025 };
            var command = new UpdateProductCommand("ff80818196f2a23f019752339d694347", "Galaxy S25 Ultra Pro", data);
            var result = await validator.TestValidateAsync(command);
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public async Task InvalidCommand()
        {
            var command = new UpdateProductCommand(string.Empty, string.Empty, null);
            var result = await validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(x => x.Id);
            result.ShouldHaveValidationErrorFor(x => x.Name);
            result.ShouldHaveValidationErrorFor(x => x.Data);
        }
    }
}
