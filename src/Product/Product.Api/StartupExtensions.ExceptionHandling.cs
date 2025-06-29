using External.Product.Core.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Diagnostics;
using System.Text.Json;

namespace External.Product.Api
{
	public static class StartupExtensions
	{
        public static void AddExceptionHandling(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    var errorFeature = context.Features.Get<IExceptionHandlerFeature>();
                    var exception = errorFeature.Error;

                    var problemDetails = new ProblemDetails
                    {
                        Type = $"https://example.com/problem-types/{exception.GetType().Name}",
                        Title = "An unexpected error occurred!",
                        Detail = "Something went wrong",
                        Instance = errorFeature switch
                        {
                            ExceptionHandlerFeature e => e.Path,
                            _ => "unknown"
                        },
                        Status = StatusCodes.Status500InternalServerError,
                        Extensions =
                        {
                            ["trace"] = Activity.Current?.Id ?? context?.TraceIdentifier
                        }
                    };

                    switch (exception)
                    {
                        case ValidationException validationException:
                            problemDetails.Status = StatusCodes.Status400BadRequest;
                            problemDetails.Title = "One or more validation errors occurred";
                            problemDetails.Detail = "The request contains invalid parameters. More information can be found in the errors.";
                            problemDetails.Extensions["errors"] = validationException.Errors;
                            break;
                        case AuthorizationException:
                            problemDetails.Status = StatusCodes.Status401Unauthorized;
                            problemDetails.Title = "Unauthorized";
                            problemDetails.Detail = "You don't have access to the resource you're trying to access";
                            break;
                        case NotFoundException:
                            problemDetails.Status = StatusCodes.Status404NotFound;
                            problemDetails.Title = "Not found";
                            problemDetails.Detail = "The resource you are looking for not found or has been deleted";
                            break;
                    }

                    context.Response.ContentType = "application/problem+json";
                    context.Response.StatusCode = problemDetails.Status.Value;
                    context.Response.GetTypedHeaders().CacheControl = new CacheControlHeaderValue()
                    {
                        NoCache = true,
                    };

                    await JsonSerializer.SerializeAsync(context.Response.Body, problemDetails);

                });
            });
        }
    }
}
