using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Salix.AspNetCore.TitlePage;

namespace WebApiTemplate.WebApi.Endpoints.Pages;

[ExcludeFromCodeCoverage]
public class FrontpageGet(IConfigurationValuesLoader configLoader) : EndpointWithoutRequest<ContentResult>
{
    public override void Configure()
    {
        Get(Urls.Pages.FrontPage);
        AllowAnonymous();
        Tags("Pages");
        Options(opts => opts.WithTags("Pages"));
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var whitelistedConfigurationValues = new HashSet<string>
        {
            "REGION",
            "WEBSITE_SITE_NAME",
            "WEBSITE_PHYSICAL_MEM",
            "ENVIRONMENT",
            "OS",
            "NUMBER_OF_PROCESSORS",
        };
#if DEBUG
        whitelistedConfigurationValues.Add("Serilog/MinimumLevel");
        whitelistedConfigurationValues.Add("AllowedHosts");
        whitelistedConfigurationValues.Add("Security");
        whitelistedConfigurationValues.Add("ConnectionStrings");
#endif
        var configurationItems = configLoader.GetConfigurationValues(whitelistedConfigurationValues);
        var obfuscatedConfig = ObfuscateConfigurationValues(configurationItems!);
        obfuscatedConfig.Add("Local Server Timezone", TimeZoneInfo.Local.DisplayName);
        obfuscatedConfig.Add("Current Culture", System.Globalization.CultureInfo.CurrentCulture.DisplayName);
        obfuscatedConfig.Add("Current UI Culture", System.Globalization.CultureInfo.CurrentUICulture.DisplayName);

        var apiAssembly = Assembly.GetAssembly(typeof(Program));
        var apiAssemblyVersion = apiAssembly!.GetName().Version ?? new Version(1, 0, 0, 0);

        var indexPage = new IndexPage()
            .SetName("WebApiTemplate")
            .SetDescription("WebApiTemplate with FastEndpoints")
            .SetHostingEnvironment(Env.EnvironmentName)
            .SetVersion(apiAssemblyVersion.ToString())    // Reads version from Assembly info
            .SetBuildTime(GetBuildTime(                   // Setting build time calculated from Assemly auto-incrementing approach
                new DateTime(2023, 1, 1, 9, 0, 0),        // Latest version "start" date - sync with date in CSPROJ
                apiAssemblyVersion.Build,
                apiAssemblyVersion.Revision))
            .AddLinkButton("Health", "/healthpage")
            .SetConfigurationValues(obfuscatedConfig)
            .IncludeContentFile("build_data.html");
#if DEBUG
        indexPage.AddLinkButton("Swagger", "/swagger/index.html");
        indexPage.SetBuildMode("#DEBUG (Should not be in production!)");
#else
        indexPage.SetBuildMode("Release");
#endif
        await SendBytesAsync(Encoding.UTF8.GetBytes(indexPage.GetContents()), contentType: "text/html", cancellation: ct);
    }

    private static DateTime GetBuildTime(DateTime versionStartDate, int daysSinceStart, int minutesSinceMidnight) =>
        versionStartDate.AddDays(daysSinceStart).Date.AddMinutes(minutesSinceMidnight).ToLocalTime();

    /// <summary>
    /// Obfuscates the configuration values for safe-ish showing on index page.
    /// </summary>
    /// <param name="configurationItems">The loaded original configuration items.</param>
    /// <returns>Same list of configuration values, where selected are obfuscated.</returns>
    private static Dictionary<string, string> ObfuscateConfigurationValues(Dictionary<string, string> configurationItems)
    {
        var obfuscated = new Dictionary<string, string>();
        foreach (var original in configurationItems)
        {
            if (original.Key.StartsWith("LogicConfiguration/SomeIp", StringComparison.OrdinalIgnoreCase)
                || original.Key.StartsWith("LogicConfiguration/SomeEmail", StringComparison.OrdinalIgnoreCase)
                || original.Key.StartsWith("LogicConfiguration/SomeArray[1].Id", StringComparison.OrdinalIgnoreCase))
            {
                obfuscated.Add(original.Key, original.Value.HideValuePartially());
                continue;
            }

            if (original.Key.StartsWith("DatabaseConnection", StringComparison.OrdinalIgnoreCase))
            {
                obfuscated.Add(original.Key, original.Value.ObfuscateSqlConnectionString(true));
                continue;
            }

            obfuscated.Add(original.Key, original.Value);
        }

        return obfuscated;
    }
}
