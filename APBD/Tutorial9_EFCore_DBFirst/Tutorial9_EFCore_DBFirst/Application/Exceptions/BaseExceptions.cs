namespace Tutorial9_EFCore_DBFirst.Exceptions;

public static class BaseExceptions
{
    public class NotFoundException(String message) : Exception(message);
}