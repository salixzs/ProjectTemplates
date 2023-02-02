using Bogus;
using WebApiTemplate.Database.Orm.Entities;

namespace WebApiTemplate.Database.Orm.Fakes.Fakes;

public static class SystemNotificationMessageRecordFake
{
    public static Faker<SystemNotificationMessageRecord> GetBogus()
    {
        var id = 0;
        return new Faker<SystemNotificationMessageRecord>()
            .RuleFor(fake => fake.Id, id++)
            .RuleFor(fake => fake.LanguageCode, faker => faker.Random.ListItem(new List<string> { "en", "de", "fr", "it", "lv" }))
            .RuleFor(fake => fake.Message, faker => faker.Random.Words(5));
    }
}
