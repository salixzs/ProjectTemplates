using ConfigurationValidation.AspNetCore;
using Salix.AspNetCore.TitlePage;
using WebApiTemplate.CoreLogic.Security;
using WebApiTemplate.Crosscut.Services;

namespace WebApiTemplate.WebApi.Bootstrap;

public static class DependencyInjectionSetup
{
    public static IServiceCollection AddApiDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterConfigurationObjects(configuration);
        CoreLogic.DependencyInjection.SetupDependencies(services, configuration);

        services.AddTransient<IConfigurationValuesLoader, ConfigurationValuesLoader>();
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddScoped<IHealthCheckResultHandler, HealthCheckResultHandler>();
        return services;
    }

    private static IServiceCollection RegisterConfigurationObjects(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureValidatableSetting<SecurityConfigurationOptions>(
            configuration.GetSection(SecurityConfigurationOptions.ConfigurationSectionName));
        return services;
    }
}
