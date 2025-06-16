using External.Product.Core.UseCases.Product.GetProducts;
using External.Product.Core.UseCases.Product.UpdateProductData;
using FluentValidation.TestHelper;
using System.Threading.Tasks;
using Xunit;

namespace External.Product.Core.UnitTests.UseCases.Product
{
    public class UpdateProductDataCommandValidatorTests
    {
        private readonly UpdateProductDataCommandValidator validator;

        public UpdateProductDataCommandValidatorTests() 
        {
            validator = new UpdateProductDataCommandValidator();
        }

        [Fact]
        public async Task ValidCommand()
        {
            var data = new Data { Capacity = "1 TB", Color = "Titanium Silverblue", Price = 1649, Year = 2025 };
            var command = new UpdateProductDataCommand("ff80818196f2a23f019752339d694347", data);
            var result = await validator.TestValidateAsync(command);
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public async Task InvalidCommand()
        {
            var command = new UpdateProductDataCommand(string.Empty, null);
            var result = await validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(x => x.Id);
            result.ShouldHaveValidationErrorFor(x => x.Data);
        }
    }
}
