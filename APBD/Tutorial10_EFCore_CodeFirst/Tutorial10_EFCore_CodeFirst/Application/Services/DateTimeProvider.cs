using Tutorial10_EFCore_CodeFirst.Application.Services.Interfaces;

namespace Tutorial10_EFCore_CodeFirst.Application.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime Now() => DateTime.Now;
}