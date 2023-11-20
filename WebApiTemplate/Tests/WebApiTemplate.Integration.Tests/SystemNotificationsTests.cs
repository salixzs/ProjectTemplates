using System.Globalization;
using System.Net.Http.Json;
using Salix.AspNetCore.JsonExceptionHandler;
using WebApiTemplate.Database.Orm;
using WebApiTemplate.Domain.SystemNotifications;
using System.Text.Json;

namespace WebApiTemplate.Integration.Tests;

public class SystemNotificationsTests(IntegrationTestFactory<Program, WebApiTemplateDbContext> factory)
        : IntegrationTestBase(factory)
{
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
        var activeNotification = DomainDataFaker.GetTestObject<SystemNotification>();
        activeNotification.StartTime = DateTimeOffset.Now.AddDays(-1);
        activeNotification.EndTime = DateTimeOffset.Now.AddDays(1);
        activeNotification.EmphasizeSince = null;
        activeNotification.CountdownSince = null;
        activeNotification.Type = Enumerations.SystemNotificationType.Normal;
        activeNotification.EmphasizeType = null;
        activeNotification.Messages.Add(DomainDataFaker.GetTestObject<SystemNotificationMessage>());
        var activeNotificationResult = await apiClient.PostAsJsonAsync(Urls.SystemNotifications.BaseUri, activeNotification);
        activeNotificationResult.Should().NotBeNull();
        activeNotificationResult.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var content = await activeNotificationResult.Content.ReadAsStringAsync();
        var activeNotificationId = Convert.ToInt32(content, CultureInfo.InvariantCulture);
        activeNotificationId.Should().BeGreaterThanOrEqualTo(1000);

        // Create 1 expired notification
        var oldNotification = DomainDataFaker.GetTestObject<SystemNotification>();
        oldNotification.StartTime = DateTimeOffset.Now.AddDays(-5);
        oldNotification.EndTime = DateTimeOffset.Now.AddDays(-4);
        oldNotification.EmphasizeSince = DateTimeOffset.Now.AddDays(-4).AddHours(-1);
        oldNotification.CountdownSince = DateTimeOffset.Now.AddDays(-4).AddMinutes(-10);
        oldNotification.Type = Enumerations.SystemNotificationType.Warning;
        oldNotification.EmphasizeType = Enumerations.SystemNotificationType.Critical;
        oldNotification.Messages.Add(DomainDataFaker.GetTestObject<SystemNotificationMessage>());
        var oldNotificationResult = await apiClient.PostAsJsonAsync(Urls.SystemNotifications.BaseUri, oldNotification);
        var oldNotificationId = Convert.ToInt32(await oldNotificationResult.Content.ReadAsStringAsync(), CultureInfo.InvariantCulture);
        oldNotificationId.Should().BeGreaterThanOrEqualTo(1000);
        oldNotificationId.Should().BeGreaterThan(activeNotificationId);

        // Active notification GET
        var activeNotifications = await apiClient.GetFromJsonAsync<List<ActiveSystemNotification>>(Urls.SystemNotifications.BaseUri);
        activeNotifications!.Should().NotBeEmpty();
        activeNotifications!.Should().HaveCount(1);
        activeNotifications![0].Id.Should().Be(activeNotificationId);

        // Specific notification GET
        var url = Urls.SystemNotifications.WithId.Replace("{id}", oldNotificationId.ToString("D", CultureInfo.InvariantCulture));
        var specificNotification = await apiClient.GetFromJsonAsync<SystemNotification>(url);
        specificNotification.Should().NotBeNull();
        specificNotification!.Id.Should().Be(oldNotificationId);

        // All notifications GET
        notifications = await apiClient.GetFromJsonAsync<IList<SystemNotification>>(Urls.SystemNotifications.All);
        notifications.Should().NotBeNull();
        notifications.Should().HaveCount(2);

        // Update active
        var updatedNotification = DomainDataFaker.GetTestObject<SystemNotification>();
        updatedNotification.Id = activeNotificationId;
        updatedNotification.StartTime = DateTimeOffset.Now.AddHours(-2);
        updatedNotification.EndTime = DateTimeOffset.Now.AddDays(2);
        updatedNotification.EmphasizeSince = DateTimeOffset.Now.AddDays(1).AddHours(12);
        updatedNotification.CountdownSince = DateTimeOffset.Now.AddDays(1).AddHours(12);
        updatedNotification.Type = Enumerations.SystemNotificationType.Warning;
        updatedNotification.EmphasizeType = Enumerations.SystemNotificationType.Critical;
        updatedNotification.Messages.Add(DomainDataFaker.GetTestObject<SystemNotificationMessage>());
        var updatedNotificationResult = await apiClient.PatchAsJsonAsync(Urls.SystemNotifications.BaseUri, updatedNotification);
        updatedNotificationResult.Should().NotBeNull();
        updatedNotificationResult.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        // Delete old notification
        var deleteNotificationResult = await apiClient.DeleteAsync(
            Urls.SystemNotifications.WithId.Replace("{id}", oldNotificationId.ToString(CultureInfo.InvariantCulture)));
        deleteNotificationResult.Should().NotBeNull();
        deleteNotificationResult.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);

        // All GET now returns only updated active
        notifications = await apiClient.GetFromJsonAsync<IList<SystemNotification>>(Urls.SystemNotifications.All);
        notifications.Should().NotBeNull();
        notifications.Should().HaveCount(1);
        notifications![0].Id.Should().Be(activeNotificationId);
        notifications![0].StartTime.Should().Be(updatedNotification.StartTime);
        notifications![0].EndTime.Should().Be(updatedNotification.EndTime);
        notifications![0].Type.Should().Be(updatedNotification.Type);
        notifications![0].EmphasizeType.Should().Be(updatedNotification.EmphasizeType);
        notifications![0].EmphasizeSince.Should().Be(updatedNotification.EmphasizeSince);
        notifications![0].CountdownSince.Should().Be(updatedNotification.CountdownSince);
        notifications![0].Messages.Should().HaveCount(1);
        notifications![0].Messages[0].Language.Should().Be(updatedNotification.Messages[0].Language);
        notifications![0].Messages[0].Message.Should().Be(updatedNotification.Messages[0].Message);
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task PostValidationError()
    {
        var apiClient = Factory.GetHttpClient();
        var failedCreateResult = await apiClient.PostAsJsonAsync(Urls.SystemNotifications.BaseUri, new SystemNotification());
        failedCreateResult.Should().NotBeNull();
        failedCreateResult.StatusCode.Should().Be(System.Net.HttpStatusCode.UnprocessableEntity);
        var apiError = await JsonSerializer.DeserializeAsync<ApiError>(
            failedCreateResult.Content.ReadAsStream(),
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        apiError.Should().NotBeNull();
        apiError!.ValidationErrors.Should().HaveCountGreaterThan(2);
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task PatchValidationError()
    {
        var apiClient = Factory.GetHttpClient();
        var failedCreateResult = await apiClient.PatchAsJsonAsync(Urls.SystemNotifications.BaseUri, new SystemNotification { Id = 1 });
        failedCreateResult.Should().NotBeNull();
        failedCreateResult.StatusCode.Should().Be(System.Net.HttpStatusCode.UnprocessableEntity);
        var apiError = await JsonSerializer.DeserializeAsync<ApiError>(
            failedCreateResult.Content.ReadAsStream(),
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        apiError.Should().NotBeNull();
        apiError!.ValidationErrors.Should().HaveCountGreaterThan(2);
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task PatchValidationError_MissingId()
    {
        var apiClient = Factory.GetHttpClient();
        var updated = DomainDataFaker.GetTestObject<SystemNotification>();
        updated.Id = 0;
        updated.StartTime = DateTimeOffset.Now.AddDays(-1);
        updated.EndTime = DateTimeOffset.Now.AddDays(1);
        updated.Messages.Add(DomainDataFaker.GetTestObject<SystemNotificationMessage>());
        var failedCreateResult = await apiClient.PatchAsJsonAsync(Urls.SystemNotifications.BaseUri, updated);
        failedCreateResult.Should().NotBeNull();
        failedCreateResult.StatusCode.Should().Be(System.Net.HttpStatusCode.UnprocessableEntity);
        var apiError = await JsonSerializer.DeserializeAsync<ApiError>(
            failedCreateResult.Content.ReadAsStream(),
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        apiError.Should().NotBeNull();
        apiError!.ValidationErrors.Should().HaveCountGreaterThan(0);
    }
}
