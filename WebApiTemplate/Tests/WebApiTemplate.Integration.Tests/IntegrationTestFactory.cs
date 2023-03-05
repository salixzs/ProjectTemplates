using System.Data;
using System.Data.Common;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace WebApiTemplate.Integration.Tests;

public class IntegrationTestFactory<TProgram, TDbContext> : WebApplicationFactory<TProgram>, IAsyncLifetime
    where TProgram : class
    where TDbContext : DbContext
{
    private readonly TestcontainerDatabase _container;

    public IntegrationTestFactory() =>
        _container = new TestcontainersBuilder<MsSqlTestcontainer>()
            .WithDatabase(
                new MsSqlTestcontainerConfiguration
                {
                    Password = "localdev#123"
                })
            .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
            .WithCleanUp(true)
            .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var connectionBuilder = new SqlConnectionStringBuilder(_container.ConnectionString)
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

    public async Task InitializeAsync() => await _container.StartAsync();

#pragma warning disable RCS1019 // Order modifiers.
    public new async Task DisposeAsync() => await _container.DisposeAsync();
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
