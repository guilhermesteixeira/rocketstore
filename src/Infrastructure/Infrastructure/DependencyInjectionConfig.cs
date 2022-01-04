namespace RocketStore.Infrastructure
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using RocketStore.Application.Common.Interfaces;
    using RocketStore.Infrastructure.Services;
    using RocketStore.Infrastructure.Storage;

    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("RocketStoreApiDb"));

            services.AddScoped<IApplicationDbContext>(provider =>
                provider.GetService<ApplicationDbContext>() ?? throw new InvalidOperationException());

            services.AddScoped<IPositionService, PositionService>();
            
            return services;
        }
    }
}