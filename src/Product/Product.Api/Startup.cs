using External.Product.Api.Filters;
using External.Product.Api.Middleware;
using External.Product.Core.Services;
using External.Product.Core.UseCases.Product.AddProduct;
using External.Product.Core.UseCases.Product.DeleteProduct;
using External.Product.Core.UseCases.Product.GetProduct;
using External.Product.Core.UseCases.Product.GetProducts;
using External.Product.Core.UseCases.Product.GetProductsByIds;
using External.Product.Core.UseCases.Product.UpdateProduct;
using External.Product.Core.UseCases.Product.UpdateProductData;
using External.Product.Core.UseCases.Product.UpdateProductName;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

namespace External.Product.Api
{
    public class Startup
	{
        readonly string AllowSpecificOrigins = "_allowSpecificOrigins";
        public IConfiguration Configuration { get; }
        private readonly IWebHostEnvironment env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
		{
            Configuration = configuration;
            this.env = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: AllowSpecificOrigins, builder =>
                {
                    builder.WithOrigins(Configuration["Cors:AllowedOrigins"]?.Split(','));
                });
            });

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorPipelineBehavior<,>));

            AssemblyScanner.FindValidatorsInAssembly(typeof(AddProductCommandValidator).Assembly)
                .ForEach(item => services.AddScoped(item.InterfaceType, item.ValidatorType));

            services.AddControllers()
                .AddJsonOptions(opt =>
                {
                    opt.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
                    opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    opt.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
                });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "External Api", Version = "v1" });
                c.OperationFilter<AuthorizeCheckOperationFilter>();
            });

            services.AddMediatR(typeof(GetProductQuery));
            services.AddMediatR(typeof(GetProductsQuery));
            services.AddMediatR(typeof(GetProductsByIdsQuery));
            services.AddMediatR(typeof(AddProductCommand));
            services.AddMediatR(typeof(UpdateProductCommand));
            services.AddMediatR(typeof(UpdateProductDataCommand));
            services.AddMediatR(typeof(UpdateProductNameCommand));
            services.AddMediatR(typeof(DeleteProductCommand));

            services.AddRouting(options =>
            {
                options.LowercaseUrls = true;
                options.LowercaseQueryStrings = true;
            });

            services.AddHealthChecks();
            services.AddHttpClient<IHttpService, HttpService>();
            services.AddHttpContextAccessor();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api v1");
                });
            }

            app.AddExceptionHandling();

            app.UseRouting();

            app.UseCors(AllowSpecificOrigins);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/api/health");
                endpoints.MapControllers();
            });
        }
    }
}
