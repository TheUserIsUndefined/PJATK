namespace Tutorial8_ADO.NET_Trans_Proceds.Services.Interfaces;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime Now() => DateTime.Now;
}