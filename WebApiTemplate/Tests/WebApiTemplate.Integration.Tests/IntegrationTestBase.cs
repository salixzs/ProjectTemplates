using Microsoft.Extensions.DependencyInjection;
using WebApiTemplate.Database.Orm;

namespace WebApiTemplate.Integration.Tests;
public class IntegrationTestBase : IClassFixture<IntegrationTestFactory<Program, WebApiTemplateDbContext>>
{
    public IntegrationTestFactory<Program, WebApiTemplateDbContext> Factory { get; }

    public WebApiTemplateDbContext DbContext { get; }

    public IntegrationTestBase(IntegrationTestFactory<Program, WebApiTemplateDbContext> factory)
    {
        Factory = factory;
        var scope = Factory.Services.CreateScope();
        DbContext = scope.ServiceProvider.GetRequiredService<WebApiTemplateDbContext>();
    }
}
