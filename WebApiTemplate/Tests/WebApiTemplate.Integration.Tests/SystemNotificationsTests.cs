using System.Globalization;
using System.Net.Http.Json;
using Azure;
using FastEndpoints;
using WebApiTemplate.Database.Orm;
using WebApiTemplate.Domain.Fakes;
using WebApiTemplate.Domain.SystemNotifications;
using WebApiTemplate.WebApi.Endpoints.SystemNotifications;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace WebApiTemplate.Integration.Tests;

public class SystemNotificationsTests : IntegrationTestBase
{
    public SystemNotificationsTests(IntegrationTestFactory<Program, WebApiTemplateDbContext> factory) : base(factory)
    {
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task FullLifecycle()
    {
        var apiClient = Factory.CreateClient();

        // No notifications exist
        //var notifications = await apiClient.GetFromJsonAsync<IList<SystemNotification>>(Urls.SystemNotifications.All);
        //notifications.Should().NotBeNull();
        //notifications.Should().BeEmpty();

        // Create 1 active notification
        var activeNotification = DomainFakesFactory.Instance.GetTestObject<SystemNotification>();
        activeNotification.StartTime = DateTimeOffset.Now.AddDays(-1);
        activeNotification.EndTime = DateTimeOffset.Now.AddDays(1);
        activeNotification.EmphasizeSince = null;
        activeNotification.CountdownSince = null;
        activeNotification.Type = Enumerations.SystemNotificationType.Normal;
        activeNotification.EmphasizeType = null;
        activeNotification.Messages.Add(DomainFakesFactory.Instance.GetTestObject<SystemNotificationMessage>());
        var activeNotificationResult = await apiClient.POSTAsync<SystemNotificationPost, SystemNotification, int>(activeNotification);
        //var content = await activeNotificationResult.Content.ReadAsStringAsync();
        //var activeNotificationId = Convert.ToInt32(content, CultureInfo.InvariantCulture);

        //var oldNotification = DomainFakesFactory.Instance.GetTestObject<SystemNotification>();
        //oldNotification.StartTime = DateTimeOffset.Now.AddDays(-5);
        //oldNotification.EndTime = DateTimeOffset.Now.AddDays(-4);
        //oldNotification.EmphasizeSince = DateTimeOffset.Now.AddDays(-4).AddHours(-1);
        //oldNotification.CountdownSince = DateTimeOffset.Now.AddDays(-4).AddMinutes(-10);
        //oldNotification.Type = Enumerations.SystemNotificationType.Warning;
        //oldNotification.EmphasizeType = Enumerations.SystemNotificationType.Critical;
        //oldNotification.Messages.Add(DomainFakesFactory.Instance.GetTestObject<SystemNotificationMessage>());
        //var oldNotificationResult = await apiClient.PostAsJsonAsync(Urls.SystemNotifications.BaseUri, oldNotification);
        //var oldNotificationId = Convert.ToInt32(await oldNotificationResult.Content.ReadAsStringAsync(), CultureInfo.InvariantCulture);
    }
}
