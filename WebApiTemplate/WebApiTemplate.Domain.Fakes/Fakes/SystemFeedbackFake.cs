using WebApiTemplate.Domain.SystemFeedbacks;
using WebApiTemplate.Enumerations;

namespace WebApiTemplate.Domain.Fakes.Fakes;

public static class SystemFeedbackFake
{
    public static Faker<SystemFeedback> GetBogus()
    {
        var id = 0;
        var status = new Faker().Random.Enum<SystemFeedbackStatus>();
        return new Faker<SystemFeedback>()
            .RuleFor(fake => fake.Id, id++)
            .RuleFor(fake => fake.Title, faker => faker.Random.Words(5))
            .RuleFor(fake => fake.Content, faker => faker.Lorem.Paragraph())
            .RuleFor(fake => fake.Category, faker => faker.Random.Enum<SystemFeedbackCategory>())
            .RuleFor(fake => fake.Status, _ => status)
            .RuleFor(fake => fake.Priority, faker => faker.Random.Enum(SystemFeedbackPriority.NotSpecified))
            .RuleFor(fake => fake.CreatedAt, _ => DateTimeOffset.Now.AddDays(-12))
            .RuleFor(fake => fake.ModifiedAt, _ => DateTimeOffset.Now.AddDays(-5))
            .RuleFor(fake => fake.CompletedAt, status == SystemFeedbackStatus.Completed ? DateTimeOffset.Now.AddDays(-1) : null)
            .RuleFor(fake => fake.SystemInfo, faker => faker.Lorem.Paragraph());
    }
}
