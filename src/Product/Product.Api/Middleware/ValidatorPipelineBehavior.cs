using FluentValidation;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace External.Product.Api.Middleware
{
	public class ValidatorPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
		where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> validators;

        public ValidatorPipelineBehavior(IEnumerable<IValidator<TRequest>> validators)
		{
			this.validators = validators;
		}

        public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var context = new ValidationContext<TRequest>(request);

            // The below is inefficient
            var validationErrors = validators
                .Select(v => v.Validate(context))
                .SelectMany(result => result.Errors)
                .ToList();

            if (validationErrors.Any())
            {
                var errors = validationErrors
                    .GroupBy(x => x.PropertyName.ToCamelCase())
                    .ToDictionary(k => k.Key, v => v.Select(x => x.ErrorMessage).ToArray());

                throw new Core.Exceptions.ValidationException(errors);
            }

            return next();
        }
    }

    public static class StringExtensions
    {
        public static string ToCamelCase(this string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return value;

            return char.ToLowerInvariant(value[0]) + value[1..];
        }
    }
}
