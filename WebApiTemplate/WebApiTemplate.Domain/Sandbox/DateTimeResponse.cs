namespace WebApiTemplate.Domain.Sandbox;

public class DateTimeResponse
{
    public DateTime LocalCurrentDateTimeValue { get; set; } = DateTime.Now;
    public DateTime UtcCurrentDateTimeValue { get; set; } = DateTime.UtcNow;
    public DateTime DateTimeValue { get; set; } = new DateTime(2023, 1, 17, 14, 15, 16, kind: DateTimeKind.Local);
    public DateOnly DateOnlyValue { get; set; } = new DateOnly(1968, 4, 3);
    public TimeOnly TimeOnlyValue { get; set; } = new TimeOnly(9, 10, 11);
    public TimeSpan TimeSpanValue { get; set; } = new TimeSpan(0, 1, 2);
}
