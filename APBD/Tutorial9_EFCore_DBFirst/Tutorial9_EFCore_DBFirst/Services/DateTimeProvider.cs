using Tutorial9_EFCore_DBFirst.Services.Abstractions;

namespace Tutorial9_EFCore_DBFirst.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime Now() => DateTime.Now;
}