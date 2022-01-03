namespace RocketStoreApi.Common
{
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using FluentValidation;
    using Hellang.Middleware.ProblemDetails;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Problem details extension configuration.
    /// </summary>
    [SuppressMessage("Resharper", "CA1062", Justification = "Is not necessary to validate the options here.")]
    public static class ProblemDetailsOptionsExtensions
    {
        /// <summary>
        /// To map the problem details configuration.
        /// </summary>
        /// <param name="options">Options.</param>
        public static void MapFluentValidationException(this ProblemDetailsOptions options) =>
            options.Map<ValidationException>((ctx, ex) =>
            {
                var factory = ctx.RequestServices.GetRequiredService<ProblemDetailsFactory>();

                var errors = ex.Errors
                    .GroupBy(x => x.PropertyName)
                    .ToDictionary(
                        x => x.Key,
                        x => x.Select(x => x.ErrorMessage).ToArray());

                return factory.CreateValidationProblemDetails(ctx, errors, StatusCodes.Status400BadRequest);
            });
    }
}