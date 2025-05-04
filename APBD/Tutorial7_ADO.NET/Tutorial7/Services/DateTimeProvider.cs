using Tutorial7.Services.Interfaces;

namespace Tutorial7.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime Now() => DateTime.Now;
}