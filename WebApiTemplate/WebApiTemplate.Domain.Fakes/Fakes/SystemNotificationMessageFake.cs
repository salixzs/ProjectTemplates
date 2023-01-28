using WebApiTemplate.Domain.SystemNotifications;

namespace Application.UnitTests.Faker.DynamicsDbFakes;

public static class SystemNotificationMessageFake
{
    public static Faker<SystemNotificationMessage> GetBogus()
    {
        var id = 0;
        return new Faker<SystemNotificationMessage>()
            .RuleFor(fake => fake.Id, id++)
            .RuleFor(fake => fake.Language, faker => faker.Random.ListItem(new List<string> { "en", "de", "lv", "no", "fi" }))
            .RuleFor(fake => fake.Message, faker => string.Concat("Maintenance break ", faker.Random.Words(3)));
    }
}
