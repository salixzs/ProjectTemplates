using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
//using WebApiTemplate.CoreLogic.Security;

namespace WebApiTemplate.CoreLogic;

public static class DependencyInjection
{
    public static void SetupDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterConfigurationObjects(configuration);
        services.RegisterHandlers();
    }

    private static IServiceCollection RegisterConfigurationObjects(this IServiceCollection services, IConfiguration configuration)
    {
        //services.ConfigureValidatableSetting<SecurityConfigurationOptions>(configuration.GetSection(SecurityConfigurationOptions.ConfigurationSectionName));
        return services;
    }

    private static IServiceCollection RegisterHandlers(this IServiceCollection services)
    {
        var handlerClasses = typeof(Handlers.Samples.WeatherForecastHandler).Assembly.GetExportedTypes()
                .Where(type =>
                    type.Namespace != null
                    && type.Namespace.StartsWith("WebApiTemplate.CoreLogic.Handlers", StringComparison.OrdinalIgnoreCase))
                .Where(type => type.GetInterfaces().Length > 0)
                .Where(type => !type.IsEnum);

        foreach (var classImplementation in handlerClasses)
        {
            foreach (var classInterface in classImplementation.GetInterfaces())
            {
                services.TryAddTransient(classInterface, classImplementation);
            }
        }

        return services;
    }
}
