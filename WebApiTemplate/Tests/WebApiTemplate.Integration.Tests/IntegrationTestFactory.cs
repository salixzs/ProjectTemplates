using System.Data;
using DotNet.Testcontainers.Builders;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;

namespace WebApiTemplate.Integration.Tests;

public class IntegrationTestFactory<TProgram, TDbContext> : WebApplicationFactory<TProgram>, IAsyncLifetime
    where TProgram : class
    where TDbContext : DbContext
{
    private readonly MsSqlContainer _sqlContainer;

    public IntegrationTestFactory() =>
        _sqlContainer = new MsSqlBuilder()
            .WithPassword("localdev#123")
            .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
            .WithWaitStrategy(Wait
                .ForUnixContainer()
                .UntilCommandIsCompleted("/opt/mssql-tools18/bin/sqlcmd", "-C", "-Q", "SELECT 1;"))
            .WithCleanUp(true)
            .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var connectionBuilder = new SqlConnectionStringBuilder(_sqlContainer.GetConnectionString())
        {
            TrustServerCertificate = true
        };

        builder.ConfigureTestServices(services =>
        {
            using (var sqlConnection = new SqlConnection(connectionBuilder.ConnectionString))
            {
                using var testDbCommand = new SqlCommand("CREATE DATABASE WebApiTemplate", sqlConnection);

                try
                {
                    sqlConnection.Open();
                    testDbCommand.ExecuteNonQuery();
                }
                catch
                {
                    throw;
                }
                finally
                {
                    if (sqlConnection.State == ConnectionState.Open)
                    {
                        sqlConnection.Close();
                    }
                }
            }

            connectionBuilder.InitialCatalog = "WebApiTemplate";
            services.RemoveDbContext<TDbContext>();
            services.AddDbContext<TDbContext>(options => options.UseSqlServer(connectionBuilder.ConnectionString));
            services.EnsureDbCreated<TDbContext>();
        });
    }

    /// <summary>
    /// Use this instead of <c>factory.CreateClient()</c> or <c>factory.CreateDefaultClient()</c> to get configured client instance in tests.
    /// </summary>
    public HttpClient GetHttpClient()
    {
        var client = CreateClient(
            new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
                BaseAddress = new Uri("https://localhost:44301")
            });
        return client;
    }

    public async Task InitializeAsync() => await _sqlContainer.StartAsync();

#pragma warning disable RCS1019 // Order modifiers.
    public new async Task DisposeAsync() => await _sqlContainer.DisposeAsync();
#pragma warning restore RCS1019
}

public static class DbContextServicesExtensions
{
    public static void RemoveDbContext<TDbContext>(this IServiceCollection services) where TDbContext : DbContext
    {
        var dbContextDescriptor = services.FirstOrDefault(d => d.ServiceType == typeof(DbContextOptions<TDbContext>));
        if (dbContextDescriptor == null)
        {
            return;
        }

        services.Remove(dbContextDescriptor);
    }

    public static void EnsureDbCreated<TDbContext>(this IServiceCollection services) where TDbContext : DbContext
    {
        var serviceProvider = services.BuildServiceProvider();

        using var scope = serviceProvider.CreateScope();
        var scopedServices = scope.ServiceProvider;
        var context = scopedServices.GetRequiredService<TDbContext>();
        context.Database.EnsureCreated();
    }
}
