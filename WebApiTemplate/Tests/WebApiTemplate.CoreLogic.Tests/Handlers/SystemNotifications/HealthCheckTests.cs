using Microsoft.Extensions.Diagnostics.HealthChecks;
using WebApiTemplate.CoreLogic.Handlers.SystemNotifications;
using WebApiTemplate.Crosscut.Services;
using WebApiTemplate.Database.Orm;
using WebApiTemplate.Database.Orm.Entities;

namespace WebApiTemplate.CoreLogic.Tests.Handlers.SystemNotifications;

public class HealthCheckTests : CoreLogicTestBase
{
    private readonly WebApiTemplateDbContext _db;

    private readonly SystemNotificationForHealthCheck _sut;

    public HealthCheckTests(ITestOutputHelper output) : base(output)
    {
        _db = GetDatabaseContext();
        DbLogger.LoggingDisabled = true;
        _sut = new SystemNotificationForHealthCheck(_db);
    }

    [Fact]
    public async Task HealthCheckProblem_CreatesSystemNotification()
    {
        await _sut.HandleHealthCheckSystemNotification(GetFakeReport(HealthStatus.Unhealthy), "http://me/hc", default);

        var queryLogic = new SystemNotificationQueries(_db, new DateTimeProvider());
        var testResult = await queryLogic.GetAll(default);
        testResult.Should().NotBeEmpty();
        testResult.Should().HaveCount(1);

        var verifyObject = testResult[0];
        verifyObject.IsHealthCheck.Should().BeTrue();
        verifyObject.StartTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(10));
        verifyObject.EndTime.Should().BeCloseTo(DateTime.UtcNow.AddHours(3), TimeSpan.FromSeconds(10));
        verifyObject.EmphasizeSince.Should().Be(verifyObject.EndTime);
        verifyObject.CountdownSince.Should().Be(verifyObject.EndTime);
        verifyObject.Type.Should().Be(Enumerations.SystemNotificationType.Warning);
        verifyObject.MoreInfoUrl.Should().Be("http://me/hc");
        verifyObject.Messages.Should().HaveCount(1);
        verifyObject.Messages[0].Language.Should().Be("en");
        verifyObject.Messages[0].Message.Should().StartWith("Health check reports Unhealthy");
    }

    [Fact]
    public async Task HealthCheckOk_NotCreateSystemNotification()
    {
        await _sut.HandleHealthCheckSystemNotification(GetFakeReport(HealthStatus.Healthy), "http://me/hc", default);

        var queryLogic = new SystemNotificationQueries(_db, new DateTimeProvider());
        var testResult = await queryLogic.GetAll(default);
        testResult.Should().BeEmpty();
    }

    [Fact]
    public async Task HealthCheckOk_RemovesSystemNotification()
    {
        _db.SystemNotifications.Add(GetOldHealthCheckRecord());
        _db.SaveChanges();

        await _sut.HandleHealthCheckSystemNotification(GetFakeReport(HealthStatus.Healthy), "http://me/hc", default);

        var queryLogic = new SystemNotificationQueries(_db, new DateTimeProvider());
        var testResult = await queryLogic.GetAll(default);
        testResult.Should().BeEmpty();
    }

    [Fact]
    public async Task HealthCheckFail_UpdatesSystemNotification()
    {
        _db.SystemNotifications.Add(GetOldHealthCheckRecord());
        _db.SaveChanges();

        await _sut.HandleHealthCheckSystemNotification(GetFakeReport(HealthStatus.Degraded), "http://me/hc2", default);

        var queryLogic = new SystemNotificationQueries(_db, new DateTimeProvider());
        var testResult = await queryLogic.GetAll(default);
        testResult.Should().NotBeEmpty();
        testResult.Should().HaveCount(1);

        var verifyObject = testResult[0];
        verifyObject.IsHealthCheck.Should().BeTrue();
        verifyObject.StartTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(10));
        verifyObject.EndTime.Should().BeCloseTo(DateTime.UtcNow.AddHours(3), TimeSpan.FromSeconds(10));
        verifyObject.EmphasizeSince.Should().Be(verifyObject.EndTime);
        verifyObject.CountdownSince.Should().Be(verifyObject.EndTime);
        verifyObject.Type.Should().Be(Enumerations.SystemNotificationType.Warning);
        verifyObject.MoreInfoUrl.Should().Be("http://me/hc2");
        verifyObject.Messages.Should().HaveCount(1);
        verifyObject.Messages[0].Language.Should().Be("en");
        verifyObject.Messages[0].Message.Should().StartWith("Health check reports Degraded");
    }

    private static HealthReport GetFakeReport(HealthStatus requiredStatus)
    {
        var entries = new Dictionary<string, HealthReportEntry>
        {
            { "test", new HealthReportEntry(requiredStatus, "some entry", TimeSpan.FromSeconds(2), null, null) }
        };
        return new HealthReport(entries.AsReadOnly(), TimeSpan.FromSeconds(3));
    }

    private static SystemNotificationRecord GetOldHealthCheckRecord()
    {
        var currentTime = DateTime.UtcNow.AddHours(-1);
        return new SystemNotificationRecord
        {
            Id = 0,
            StartTime = currentTime,
            EndTime = currentTime.AddHours(3),
            CountdownSince = currentTime.AddHours(3),
            EmphasizeSince = currentTime.AddHours(3),
            Type = Enumerations.SystemNotificationType.Warning,
            EmphasizeType = Enumerations.SystemNotificationType.Warning,
            IsHealthCheck = true,
            MoreInfoUrl = "http://old.hc",
            Messages = new List<SystemNotificationMessageRecord>
            {
                new SystemNotificationMessageRecord
                {
                    LanguageCode = "en",
                    Message = "Old message"
                }
            }
        };
    }
}
