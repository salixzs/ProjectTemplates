using WebApiTemplate.Domain.SystemNotifications;
using WebApiTemplate.Enumerations;

namespace WebApiTemplate.Domain.Fakes.Fakes;

public static class SystemNotificationFake
{
    public static Faker<SystemNotification> GetBogus()
    {
        var id = 0;
        return new Faker<SystemNotification>()
            .RuleFor(fake => fake.Id, id++)
            .RuleFor(fake => fake.StartTime, DateTime.UtcNow)
            .RuleFor(fake => fake.EndTime, faker => DateTime.UtcNow.AddMinutes(faker.Random.Int(5, 30)))
            .RuleFor(fake => fake.EmphasizeSince, faker => DateTime.UtcNow.AddMinutes(faker.Random.Int(15, 25)))
            .RuleFor(fake => fake.CountdownSince, faker => DateTime.UtcNow.AddMinutes(faker.Random.Int(20, 27)))
            .RuleFor(fake => fake.Type, faker => faker.Random.Enum<SystemNotificationType>())
            .RuleFor(fake => fake.EmphasizeType, faker => faker.Random.Enum<SystemNotificationType>())
            .RuleFor(fake => fake.IsHealthCheck, false)
            .RuleFor(fake => fake.MoreInfoUrl, faker => faker.Internet.Url());
    }
}
