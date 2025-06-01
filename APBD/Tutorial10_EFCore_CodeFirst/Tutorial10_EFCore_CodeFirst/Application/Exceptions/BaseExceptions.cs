namespace Tutorial10_EFCore_CodeFirst.Application.Exceptions;

public static class BaseExceptions
{
    public class NotFoundException(string message) : Exception(message);
}