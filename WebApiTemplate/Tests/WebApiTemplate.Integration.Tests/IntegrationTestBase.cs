using Microsoft.Extensions.DependencyInjection;
using WebApiTemplate.Database.Orm;
using WebApiTemplate.Domain.Fakes;
using WebApiTemplate.WebApi;

namespace WebApiTemplate.Integration.Tests;

public class IntegrationTestBase : IClassFixture<IntegrationTestFactory<Program, WebApiTemplateDbContext>>
{
    private DomainFakesFactory? _domainObjectsFakesFactory;

    /// <summary>
    /// Use Factory in tests to get fakes of Domain objects and their lists.
    /// <code>
    /// GetTestObject&lt;User&gt;();
    /// GetTestObjects&lt;Account&gt;(10, 30);
    /// </code>
    /// </summary>
    protected DomainFakesFactory DomainDataFaker =>
        _domainObjectsFakesFactory ??= new DomainFakesFactory();

    public IntegrationTestFactory<Program, WebApiTemplateDbContext> Factory { get; }

    public WebApiTemplateDbContext DbContext { get; }

    public IntegrationTestBase(IntegrationTestFactory<Program, WebApiTemplateDbContext> factory)
    {
        Factory = factory;
        var scope = Factory.Services.CreateScope();
        DbContext = scope.ServiceProvider.GetRequiredService<WebApiTemplateDbContext>();
    }
}
