namespace WebApiTemplate.BootSetup;

public static class DependencyInjectionSetup
{
    public static IServiceCollection AddApiDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        CoreLogic.DependencyInjection.SetupDependencies(services, configuration);
        return services;
    }
}
