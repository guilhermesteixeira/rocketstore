namespace RocketStoreApi
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using AutoMapper;
    using Hellang.Middleware.ProblemDetails;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using RocketStore.Application;
    using RocketStore.Application.Common.Exceptions;
    using RocketStore.Application.Managers;
    using RocketStore.Infrastructure;
    using RocketStoreApi.Common;

    /// <summary>
    /// Defines the startup of the application.
    /// </summary>
    public class Startup
    {
        private readonly IConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">Configuration object.</param>
        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        /// <summary>
        /// Configures the services required by the application.
        /// </summary>
        /// <param name="services">The service collection.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddInfrastructure();
            services.AddApplication();

            services.AddProblemDetails(this.ConfigureProblemDetails);
            services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.IgnoreNullValues = true);

            services.AddControllers();

            services.AddOpenApiDocument(
                (options) =>
                {
                    options.DocumentName = "Version 1";
                    options.Title = "RocketStore API";
                    options.Description = "REST API for the RocketStore Web Application";
                });

            services
                .AddAutoMapper(
                    (provider, options) =>
                    {
                        foreach (Profile profile in provider.GetServices<Profile>())
                        {
                            options.AddProfile(profile);
                        }
                    },
                    assemblies: null);
        }

        /// <summary>
        /// Configures the application.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="env">The environment.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseProblemDetails();

            app.UseOpenApi();

            app.UseSwaggerUi3();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void ConfigureProblemDetails(ProblemDetailsOptions options)
        {
            options.IncludeExceptionDetails = (ctx, ex) => false;

            options.MapFluentValidationException();
            options.MapToStatusCode<HttpRequestException>(StatusCodes.Status503ServiceUnavailable);
            options.MapToStatusCode<NotFoundException>(StatusCodes.Status404NotFound);
            options.MapToStatusCode<UnauthorizedAccessException>(StatusCodes.Status401Unauthorized);
            options.Map<CustomerAlreadyExistsException>((context, exception) =>
            {
                var factory = context.RequestServices.GetRequiredService<ProblemDetailsFactory>();
                var details = factory.CreateValidationProblemDetails(context, errors: new Dictionary<string, string[]>());
                details.Title = ErrorCodes.CustomerAlreadyExists;
                details.Status = StatusCodes.Status409Conflict;

                return details;
            });

            options.MapToStatusCode<Exception>(StatusCodes.Status500InternalServerError);
        }
    }
}
