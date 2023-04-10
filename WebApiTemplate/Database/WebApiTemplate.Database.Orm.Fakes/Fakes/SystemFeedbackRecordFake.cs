using Bogus;
using WebApiTemplate.Database.Orm.Entities;
using WebApiTemplate.Enumerations;

namespace WebApiTemplate.Database.Orm.Fakes.Fakes;

public static class SystemFeedbackRecordFake
{
    public static Faker<SystemFeedbackRecord> GetBogus()
    {
        var id = 0;
        var createTime = DateTimeOffset.Now.AddDays(-3);
        var status = new Faker().Random.Enum<SystemFeedbackStatus>();
        DateTimeOffset? completedDate = status == SystemFeedbackStatus.Completed ? createTime.AddDays(2) : null;
        return new Faker<SystemFeedbackRecord>()
            .RuleFor(fake => fake.Id, id++)
            .RuleFor(fake => fake.Title, faker => faker.Random.Words(4))
            .RuleFor(fake => fake.Content, faker => faker.Lorem.Paragraph(1))
            .RuleFor(fake => fake.SystemInfo, faker => faker.Lorem.Paragraph(3))
            .RuleFor(fake => fake.CreatedAt, _ => createTime)
            .RuleFor(fake => fake.ModifiedAt, _ => createTime)
            .RuleFor(fake => fake.CompletedAt, _ => completedDate)
            .RuleFor(fake => fake.Category, faker => faker.Random.Enum<SystemFeedbackCategory>())
            .RuleFor(fake => fake.Status, _ => status)
            .RuleFor(fake => fake.Priority, faker => faker.Random.Enum(SystemFeedbackPriority.NotSpecified));
    }
}
