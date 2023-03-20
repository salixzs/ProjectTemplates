using Bogus;
using WebApiTemplate.Database.Orm.Entities;
using WebApiTemplate.Enumerations;

namespace WebApiTemplate.Database.Orm.Fakes.Fakes;

public static class SystemNotificationRecordFake
{
    public static Faker<SystemNotificationRecord> GetBogus()
    {
        var id = 0;
        return new Faker<SystemNotificationRecord>()
            .RuleFor(fake => fake.Id, id++)
            .RuleFor(fake => fake.StartTime, DateTimeOffset.Now)
            .RuleFor(fake => fake.EndTime, faker => DateTimeOffset.Now.AddMinutes(faker.Random.Int(5, 30)))
            .RuleFor(fake => fake.EmphasizeSince, faker => DateTimeOffset.Now.AddMinutes(faker.Random.Int(15, 25)))
            .RuleFor(fake => fake.CountdownSince, faker => DateTimeOffset.Now.AddMinutes(faker.Random.Int(20, 27)))
            .RuleFor(fake => fake.Type, faker => faker.Random.Enum<SystemNotificationType>())
            .RuleFor(fake => fake.EmphasizeType, faker => faker.Random.Enum<SystemNotificationType>())
            .RuleFor(fake => fake.IsHealthCheck, false)
            .RuleFor(fake => fake.MoreInfoUrl, faker => faker.Internet.Url());
    }
}
