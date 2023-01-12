using ConfigurationValidation.AspNetCore;
using Salix.AspNetCore.TitlePage;
using WebApiTemplate.CoreLogic.Security;

namespace WebApiTemplate.BootSetup;

public static class DependencyInjectionSetup
{
    public static IServiceCollection AddApiDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterConfigurationObjects(configuration);
        CoreLogic.DependencyInjection.SetupDependencies(services, configuration);

        services.AddTransient<IConfigurationValuesLoader, ConfigurationValuesLoader>();
        return services;
    }

    private static IServiceCollection RegisterConfigurationObjects(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureValidatableSetting<SecurityConfigurationOptions>(configuration.GetSection(SecurityConfigurationOptions.ConfigurationSectionName));
        return services;
    }
}
