using WebApiTemplate.Domain.SystemNotifications;
using WebApiTemplate.Enumerations;

namespace WebApiTemplate.Domain.Fakes.Fakes;

public static class ActiveSystemNotificationFake
{
    public static Faker<ActiveSystemNotification> GetBogus()
    {
        var id = 0;
        return new Faker<ActiveSystemNotification>()
            .RuleFor(fake => fake.Id, id++)
            .RuleFor(fake => fake.EndTime, faker => DateTime.UtcNow.AddMinutes(faker.Random.Int(5, 30)))
            .RuleFor(fake => fake.IsEmphasized, faker => faker.Random.Bool())
            .RuleFor(fake => fake.ShowCountdown, faker => faker.Random.Bool())
            .RuleFor(fake => fake.MessageType, faker => faker.Random.Enum<SystemNotificationType>());
    }
}
