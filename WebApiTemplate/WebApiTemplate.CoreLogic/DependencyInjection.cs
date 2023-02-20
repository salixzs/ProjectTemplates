using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WebApiTemplate.Database.Orm;

namespace WebApiTemplate.CoreLogic;

[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    public static void SetupDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        ContextRegistration.AddDatabaseContext(services, configuration);
        services.RegisterHandlers();
    }

    /// <summary>
    /// Automatic registration of Interface+Implementation pairs under Handlers namespace.
    /// </summary>
    private static IServiceCollection RegisterHandlers(this IServiceCollection services)
    {
        var handlerClasses = typeof(Handlers.Samples.WeatherForecastHandler).Assembly.GetExportedTypes()
                .Where(type =>
                    type.Namespace?.StartsWith("WebApiTemplate.CoreLogic.Handlers", StringComparison.OrdinalIgnoreCase) == true
                        && type.GetInterfaces().Length > 0
                        && !type.IsEnum);

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
