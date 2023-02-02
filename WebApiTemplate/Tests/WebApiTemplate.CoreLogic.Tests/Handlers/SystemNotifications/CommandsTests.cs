using WebApiTemplate.CoreLogic.Handlers.SystemNotifications;
using WebApiTemplate.Crosscut.Services;
using WebApiTemplate.Database.Orm;
using WebApiTemplate.Domain.Fakes;
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
        _sut = new SystemNotificationCommands(_db);
    }

    [Fact]
    public async Task Create_Succeeds()
    {
        var testNotification = DomainFakesFactory.Instance.GetTestObject<SystemNotification>();
        testNotification.Messages.Add(DomainFakesFactory.Instance.GetTestObject<SystemNotificationMessage>());
        var result = await _sut.Create(testNotification, default);
        result.Should().Be(1);

        var queryLogic = new SystemNotificationQueries(_db, new DateTimeProvider());
        var testResult = await queryLogic.GetAll(default);
        testResult.Should().NotBeEmpty();
        testResult.Should().HaveCount(1);

        var verifyObject = testResult.First();
        verifyObject.Id.Should().Be(1);
        verifyObject.Messages.Should().HaveCount(1);
        verifyObject.StartTime.Should().BeCloseTo(testNotification.StartTime, TimeSpan.FromMilliseconds(1));
        verifyObject.EndTime.Should().BeCloseTo(testNotification.EndTime, TimeSpan.FromMilliseconds(1));
        verifyObject.EmphasizeSince.Should().BeCloseTo(testNotification.EmphasizeSince!.Value, TimeSpan.FromMilliseconds(1));
        verifyObject.CountdownSince.Should().BeCloseTo(testNotification.CountdownSince!.Value, TimeSpan.FromMilliseconds(1));
        verifyObject.Type.Should().Be(testNotification.Type);
        verifyObject.EmphasizeType.Should().Be(testNotification.EmphasizeType);
    }
}
