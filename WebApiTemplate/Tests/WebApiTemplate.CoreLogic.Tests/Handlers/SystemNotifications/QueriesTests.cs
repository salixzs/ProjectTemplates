using WebApiTemplate.CoreLogic.Handlers.SystemNotifications;
using WebApiTemplate.Crosscut.Services;
using WebApiTemplate.Database.Orm;
using WebApiTemplate.Database.Orm.Entities;

namespace WebApiTemplate.CoreLogic.Tests.Handlers.SystemNotifications;

public class QueriesTests : CoreLogicTestBase
{
    private readonly WebApiTemplateDbContext _db;

    private readonly SystemNotificationQueries _sut;

    private readonly Mock<IDateTimeProvider> _dateTimeProvider;

    private SystemNotificationRecord _testable = null!;

    private readonly DateTimeOffset _testableBaseDateTime = DateTimeOffset.Now.AddDays(1);

    public QueriesTests(ITestOutputHelper output) : base(output)
    {
        _db = GetDatabaseContext();
        DbLogger.LoggingDisabled = true;
        PrepareDatabase();
        _dateTimeProvider = new Mock<IDateTimeProvider>();
        _sut = new SystemNotificationQueries(_db, _dateTimeProvider.Object);
    }

    [Fact]
    public async Task QuerySingle_ValidId_ReturnsData()
    {
        var result = await _sut.GetById(1011, default);
        result.Should().NotBeNull();
        result!.Messages.Should().HaveCount(1);
    }

    [Fact]
    public async Task QuerySingle_WrongId_ReturnsNull()
    {
        var result = await _sut.GetById(2000, default);
        result.Should().BeNull();
    }

    [Fact]
    public async Task QueryActive_ReturnsOne()
    {
        _dateTimeProvider.Setup(dt => dt.DateTimeOffsetNow).Returns(DateTimeOffset.Now);
        var result = await _sut.GetActive(default);
        result.Should().NotBeEmpty();
        result.Should().HaveCount(1);
        result[0].Id.Should().Be(1010);
        result![0].Messages.Should().HaveCount(1);
    }

    [Fact]
    public async Task QueryAll_ReturnsThree()
    {
        var result = await _sut.GetAll(default);
        result.Should().NotBeEmpty();
        result.Should().HaveCount(3);
        result![0].Messages.Should().HaveCountGreaterThan(0);
    }

    [Fact]
    public async Task QueryActive_Mapping_Correct()
    {
        _dateTimeProvider.Setup(dt => dt.DateTimeOffsetNow).Returns(_testableBaseDateTime.AddMinutes(1));
        var result = await _sut.GetActive(default);
        result.Should().NotBeEmpty();
        result.Should().HaveCount(1);
        var resultItem = result[0];
        resultItem.Id.Should().Be(1012);
        resultItem.IsEmphasized.Should().BeFalse();
        resultItem.ShowCountdown.Should().BeFalse();
        resultItem.MessageType.Should().Be(Enumerations.SystemNotificationType.Normal);
        resultItem.Messages.Should().HaveCount(2);
    }

    [Fact]
    public async Task QueryActive_Emphasized_Correct()
    {
        _dateTimeProvider.Setup(dt => dt.DateTimeOffsetNow).Returns(_testableBaseDateTime.AddMinutes(16));
        var result = await _sut.GetActive(default);
        result.Should().NotBeEmpty();
        result.Should().HaveCount(1);
        var resultItem = result[0];
        resultItem.Id.Should().Be(1012);
        resultItem.IsEmphasized.Should().BeTrue();
        resultItem.ShowCountdown.Should().BeFalse();
        resultItem.MessageType.Should().Be(Enumerations.SystemNotificationType.Warning);
        resultItem.Messages.Should().HaveCount(2);
    }

    [Fact]
    public async Task QueryActive_Countdown_Correct()
    {
        _dateTimeProvider.Setup(dt => dt.DateTimeOffsetNow).Returns(_testableBaseDateTime.AddMinutes(19));
        var result = await _sut.GetActive(default);
        result.Should().NotBeEmpty();
        result.Should().HaveCount(1);
        var resultItem = result[0];
        resultItem.Id.Should().Be(1012);
        resultItem.IsEmphasized.Should().BeTrue();
        resultItem.ShowCountdown.Should().BeTrue();
        resultItem.MessageType.Should().Be(Enumerations.SystemNotificationType.Warning);
        resultItem.Messages.Should().HaveCount(2);
    }

    private void PrepareDatabase()
    {
        // Active
        var sn1 = DatabaseDataFaker.GetTestObject<SystemNotificationRecord>();
        var sn1m = DatabaseDataFaker.GetTestObject<SystemNotificationMessageRecord>();
        sn1.Id = 1010;
        sn1.Messages.Add(sn1m);

        // Inactive
        var sn2 = DatabaseDataFaker.GetTestObject<SystemNotificationRecord>();
        sn2.Id = 1011;
        sn2.StartTime = DateTimeOffset.Now.AddDays(-1);
        sn2.EndTime = DateTimeOffset.Now.AddDays(-1).AddMinutes(Random.Shared.Next(5, 30));
        sn2.EmphasizeSince = DateTimeOffset.Now.AddDays(-1).AddMinutes(Random.Shared.Next(15, 25));
        sn2.CountdownSince = DateTimeOffset.Now.AddDays(-1).AddMinutes(Random.Shared.Next(20, 27));
        var sn2m = DatabaseDataFaker.GetTestObject<SystemNotificationMessageRecord>();
        sn2.Messages.Add(sn2m);

        // Testable
        _testable = new SystemNotificationRecord
        {
            Id = 1012,
            StartTime = _testableBaseDateTime,
            EndTime = _testableBaseDateTime.AddMinutes(20),
            EmphasizeSince = _testableBaseDateTime.AddMinutes(15),
            CountdownSince = _testableBaseDateTime.AddMinutes(18),
            Type = Enumerations.SystemNotificationType.Normal,
            EmphasizeType = Enumerations.SystemNotificationType.Warning
        };
        _testable.Messages.Add(new SystemNotificationMessageRecord { Id = 2010, LanguageCode = "lv", Message = "Meža ķēniņš un rūķīši." });
        _testable.Messages.Add(new SystemNotificationMessageRecord { Id = 2011, LanguageCode = "en", Message = "Brown Fox and Sleeping Dog." });

        _db.SystemNotifications.Add(sn1);
        _db.SystemNotifications.Add(sn2);
        _db.SystemNotifications.Add(_testable);
        _db.SaveChanges();
        DbLogger.LoggingDisabled = false;
    }
}
