using System.Globalization;
using System.Net.Http.Json;
using WebApiTemplate.Database.Orm;
using WebApiTemplate.Domain.Fakes;
using WebApiTemplate.Domain.SystemNotifications;

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
        var apiClient = Factory.GetHttpClient();

        // No notifications exist
        var notifications = await apiClient.GetFromJsonAsync<IList<SystemNotification>>(Urls.SystemNotifications.All);
        notifications.Should().NotBeNull();
        notifications.Should().BeEmpty();

        // Create 1 active notification
        var activeNotification = DomainFakesFactory.Instance.GetTestObject<SystemNotification>();
        activeNotification.StartTime = DateTimeOffset.Now.AddDays(-1);
        activeNotification.EndTime = DateTimeOffset.Now.AddDays(1);
        activeNotification.EmphasizeSince = null;
        activeNotification.CountdownSince = null;
        activeNotification.Type = Enumerations.SystemNotificationType.Normal;
        activeNotification.EmphasizeType = null;
        activeNotification.Messages.Add(DomainFakesFactory.Instance.GetTestObject<SystemNotificationMessage>());
        var activeNotificationResult = await apiClient.PostAsJsonAsync(Urls.SystemNotifications.BaseUri, activeNotification);
        activeNotificationResult.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var content = await activeNotificationResult.Content.ReadAsStringAsync();
        var activeNotificationId = Convert.ToInt32(content, CultureInfo.InvariantCulture);
        activeNotificationId.Should().BeGreaterThanOrEqualTo(1000);

        // Create 1 expired notification
        var oldNotification = DomainFakesFactory.Instance.GetTestObject<SystemNotification>();
        oldNotification.StartTime = DateTimeOffset.Now.AddDays(-5);
        oldNotification.EndTime = DateTimeOffset.Now.AddDays(-4);
        oldNotification.EmphasizeSince = DateTimeOffset.Now.AddDays(-4).AddHours(-1);
        oldNotification.CountdownSince = DateTimeOffset.Now.AddDays(-4).AddMinutes(-10);
        oldNotification.Type = Enumerations.SystemNotificationType.Warning;
        oldNotification.EmphasizeType = Enumerations.SystemNotificationType.Critical;
        oldNotification.Messages.Add(DomainFakesFactory.Instance.GetTestObject<SystemNotificationMessage>());
        var oldNotificationResult = await apiClient.PostAsJsonAsync(Urls.SystemNotifications.BaseUri, oldNotification);
        content = await oldNotificationResult.Content.ReadAsStringAsync();
        var oldNotificationId = Convert.ToInt32(await oldNotificationResult.Content.ReadAsStringAsync(), CultureInfo.InvariantCulture);
        oldNotificationId.Should().BeGreaterThanOrEqualTo(1000);
        oldNotificationId.Should().BeGreaterThan(activeNotificationId);

        var activeNotifications = await apiClient.GetFromJsonAsync<List<ActiveSystemNotification>>(Urls.SystemNotifications.BaseUri);
        activeNotifications!.Should().NotBeEmpty();
        activeNotifications!.Should().HaveCount(1);
        activeNotifications![0].Id.Should().Be(activeNotificationId);

        var url = Urls.SystemNotifications.WithId.Replace("{id}", oldNotificationId.ToString("D"));
        var specificNotification = await apiClient.GetFromJsonAsync<SystemNotification>(url);
        specificNotification.Should().NotBeNull();
        specificNotification!.Id.Should().Be(oldNotificationId);

        notifications = await apiClient.GetFromJsonAsync<IList<SystemNotification>>(Urls.SystemNotifications.All);
        notifications.Should().NotBeNull();
        notifications.Should().HaveCount(2);
    }
}
