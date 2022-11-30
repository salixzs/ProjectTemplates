namespace WebApiTemplate.BootSetup;

public static class DependencyInjectionSetup
{
    public static IServiceCollection AddApiDependencies(this IServiceCollection services, ConfigurationManager configuration)
    {
        CoreLogic.DependencyInjection.SetupDependencies(services);
        return services;
    }
}
