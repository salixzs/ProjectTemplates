using Microsoft.EntityFrameworkCore;
using WebApiTemplate.CoreLogic.Handlers.SystemNotifications;
using WebApiTemplate.Crosscut.Services;
using WebApiTemplate.Database.Orm;
using WebApiTemplate.Database.Orm.Entities;
using WebApiTemplate.Domain.SystemNotifications;

namespace WebApiTemplate.CoreLogic.Tests.Handlers.SystemNotifications;

public class CommandsTests : CoreLogicTestBase
{
    private readonly WebApiTemplateDbContext _db;

    private readonly SystemNotificationCommands _sut;

    public CommandsTests(ITestOutputHelper output) : base(output)
    {
        _db = GetDatabaseContext();
        DbLogger.LoggingDisabled = true;
        _sut = new SystemNotificationCommands(_db, DefaultMocks.GetErrorTranslationMock().Object);
    }

    [Fact]
    public async Task Create_Succeeds()
    {
        var testNotification = DomainDataFaker.GetTestObject<SystemNotification>();
        testNotification.Messages.Add(DomainDataFaker.GetTestObject<SystemNotificationMessage>());
        var result = await _sut.Create(testNotification, default);
        result.Should().Be(1);

        var queryLogic = new SystemNotificationQueries(_db, new DateTimeProvider());
        var testResult = await queryLogic.GetAll(default);
        testResult.Should().NotBeEmpty();
        testResult.Should().HaveCount(1);

        var verifyObject = testResult[0];
        verifyObject.Id.Should().Be(1);
        verifyObject.Messages.Should().HaveCount(1);
        verifyObject.StartTime.Should().BeCloseTo(testNotification.StartTime, TimeSpan.FromMilliseconds(1));
        verifyObject.EndTime.Should().BeCloseTo(testNotification.EndTime, TimeSpan.FromMilliseconds(1));
        verifyObject.EmphasizeSince.Should().BeCloseTo(testNotification.EmphasizeSince!.Value, TimeSpan.FromMilliseconds(1));
        verifyObject.CountdownSince.Should().BeCloseTo(testNotification.CountdownSince!.Value, TimeSpan.FromMilliseconds(1));
        verifyObject.Type.Should().Be(testNotification.Type);
        verifyObject.EmphasizeType.Should().Be(testNotification.EmphasizeType);
    }

    [Fact]
    public async Task Edit_Succeeds()
    {
        var initialNotification = new SystemNotificationRecord
        {
            Id = 2000,
            StartTime = new DateTimeOffset(2022, 2, 2, 12, 2, 2, TimeSpan.Zero),
            EndTime = new DateTimeOffset(2022, 3, 3, 13, 3, 3, TimeSpan.Zero),
            Type = Enumerations.SystemNotificationType.Normal,
            EmphasizeSince = new DateTimeOffset(2022, 3, 1, 11, 1, 1, TimeSpan.Zero),
            EmphasizeType = Enumerations.SystemNotificationType.Success,
            CountdownSince = new DateTimeOffset(2022, 3, 1, 14, 4, 4, TimeSpan.Zero),
        };
        initialNotification.Messages.Add(DatabaseDataFaker.GetTestObject<SystemNotificationMessageRecord>());
        _db.SystemNotifications.Add(initialNotification);
        _db.SaveChanges();

        var updateNotification = new SystemNotification
        {
            Id = 2000,
            StartTime = new DateTimeOffset(2022, 5, 5, 15, 5, 5, new TimeSpan(2, 0, 0)),
            EndTime = new DateTimeOffset(2022, 6, 6, 16, 6, 6, new TimeSpan(2, 0, 0)),
            Type = Enumerations.SystemNotificationType.Warning,
            EmphasizeSince = new DateTimeOffset(2022, 6, 5, 16, 6, 6, new TimeSpan(2, 0, 0)),
            EmphasizeType = Enumerations.SystemNotificationType.Critical,
            CountdownSince = new DateTimeOffset(2022, 6, 5, 17, 7, 7, new TimeSpan(2, 0, 0)),
        };
        updateNotification.Messages.Add(new SystemNotificationMessage { Language = "mx", Message = "Burritos and Tacos" });

        await _sut.Update(updateNotification, default);

        var queryLogic = new SystemNotificationQueries(_db, new DateTimeProvider());
        var testResult = await queryLogic.GetAll(default);
        testResult.Should().NotBeEmpty();
        testResult.Should().HaveCount(1);

        var verifyObject = testResult[0];
        verifyObject.Id.Should().Be(2000);
        verifyObject.Messages.Should().HaveCount(1);
        verifyObject.StartTime.Should().BeCloseTo(updateNotification.StartTime, TimeSpan.FromSeconds(1));
        verifyObject.EndTime.Should().BeCloseTo(updateNotification.EndTime, TimeSpan.FromSeconds(1));
        verifyObject.EmphasizeSince.Should().BeCloseTo(updateNotification.EmphasizeSince!.Value, TimeSpan.FromSeconds(1));
        verifyObject.CountdownSince.Should().BeCloseTo(updateNotification.CountdownSince!.Value, TimeSpan.FromSeconds(1));
        verifyObject.Type.Should().Be(updateNotification.Type);
        verifyObject.EmphasizeType.Should().Be(updateNotification.EmphasizeType);

        var verifyMessage = verifyObject.Messages[0];
        verifyMessage.Language.Should().Be("mx");
        verifyMessage.Message.Should().Be("Burritos and Tacos");
    }

    [Fact]
    public async Task Edit_DefaultValues_Succeeds()
    {
        var initialNotification = new SystemNotificationRecord
        {
            Id = 2000,
            StartTime = new DateTimeOffset(2022, 2, 2, 12, 2, 2, TimeSpan.Zero),
            EndTime = new DateTimeOffset(2022, 3, 3, 13, 3, 3, TimeSpan.Zero),
            Type = Enumerations.SystemNotificationType.Normal,
            EmphasizeSince = new DateTimeOffset(2022, 3, 1, 11, 1, 1, TimeSpan.Zero),
            EmphasizeType = Enumerations.SystemNotificationType.Success,
            CountdownSince = new DateTimeOffset(2022, 3, 1, 14, 4, 4, TimeSpan.Zero),
        };
        initialNotification.Messages.Add(DatabaseDataFaker.GetTestObject<SystemNotificationMessageRecord>());
        _db.SystemNotifications.Add(initialNotification);
        _db.SaveChanges();

        var updateNotification = new SystemNotification
        {
            Id = 2000,
            StartTime = new DateTimeOffset(2022, 5, 5, 15, 5, 5, new TimeSpan(2, 0, 0)),
            EndTime = new DateTimeOffset(2022, 6, 6, 16, 6, 6, new TimeSpan(2, 0, 0)),
            Type = Enumerations.SystemNotificationType.Warning,
            EmphasizeSince = null,
            EmphasizeType = null,
            CountdownSince = null,
        };
        updateNotification.Messages.Add(new SystemNotificationMessage { Language = "mx", Message = "Burritos and Tacos" });

        await _sut.Update(updateNotification, default);

        var queryLogic = new SystemNotificationQueries(_db, new DateTimeProvider());
        var testResult = await queryLogic.GetAll(default);
        testResult.Should().NotBeEmpty();
        testResult.Should().HaveCount(1);

        var verifyObject = testResult[0];
        verifyObject.EmphasizeSince.Should().BeCloseTo(updateNotification.EndTime, TimeSpan.FromSeconds(1));
        verifyObject.CountdownSince.Should().BeCloseTo(updateNotification.EndTime, TimeSpan.FromSeconds(1));
        verifyObject.EmphasizeType.Should().Be(updateNotification.Type);
    }

    [Fact]
    public async Task Delete_Succeeds()
    {
        var initialNotification = new SystemNotificationRecord
        {
            Id = 2000,
            StartTime = new DateTimeOffset(2022, 2, 2, 12, 2, 2, TimeSpan.Zero),
            EndTime = new DateTimeOffset(2022, 3, 3, 13, 3, 3, TimeSpan.Zero),
            Type = Enumerations.SystemNotificationType.Normal,
            EmphasizeSince = new DateTimeOffset(2022, 3, 1, 11, 1, 1, TimeSpan.Zero),
            EmphasizeType = Enumerations.SystemNotificationType.Success,
            CountdownSince = new DateTimeOffset(2022, 3, 1, 14, 4, 4, TimeSpan.Zero),
        };
        initialNotification.Messages.Add(DatabaseDataFaker.GetTestObject<SystemNotificationMessageRecord>());
        _db.SystemNotifications.Add(initialNotification);
        _db.SaveChanges();

        await _sut.Delete(2000, default);

        var dbNotifications = await _db.SystemNotifications.ToListAsync();
        dbNotifications.Should().BeEmpty();

        var dbMessages = await _db.SystemNotificationMessages.ToListAsync();
        dbMessages.Should().BeEmpty();
    }
}
