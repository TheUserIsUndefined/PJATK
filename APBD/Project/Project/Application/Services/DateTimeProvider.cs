using Project.Application.Services.Interfaces;

namespace Project.Application.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime Now() => DateTime.Now;
    public DateOnly Today() => DateOnly.FromDateTime(DateTime.Now);
}