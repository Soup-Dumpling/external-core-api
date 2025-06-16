using External.Product.Core.UseCases.Product.AddProduct;
using External.Product.Core.UseCases.Product.GetProducts;
using FluentValidation.TestHelper;
using System.Threading.Tasks;
using Xunit;

namespace External.Product.Core.UnitTests.UseCases.Product
{
    public class AddProductCommandValidatorTests
    {
        private readonly AddProductCommandValidator validator;

        public AddProductCommandValidatorTests() 
        {
            validator = new AddProductCommandValidator();
        }

        [Fact]
        public async Task ValidCommand()
        {
            var data = new Data { Capacity = "512 GB", Color = "Titanium Silverblue", Price = 1349, Year = 2025 };
            var command = new AddProductCommand("Galaxy S25 Ultra", data);
            var result = await validator.TestValidateAsync(command);
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public async Task InvalidCommand()
        {
            var command = new AddProductCommand(string.Empty, null);
            var result = await validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(x => x.Name);
            result.ShouldHaveValidationErrorFor(x => x.Data);
        }
    }
}
