namespace RocketStore.Application
{
    using System.Reflection;
    using AutoMapper;
    using FluentValidation;
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;
    using RocketStore.Application.Common.Behaviours;
    using RocketStore.Application.Common.Mappings;
    using RocketStore.Application.Managers;

    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddSingleton<Profile, MappingProfile>();
            
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
            
            return services;
        }
    }
}