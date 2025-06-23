namespace Project.Application.Services.Interfaces;

public interface IDateTimeProvider
{
    public DateTime Now();
    public DateOnly Today();
}