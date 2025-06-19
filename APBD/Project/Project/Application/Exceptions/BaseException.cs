namespace Project.Application.Exceptions;

public class BaseException
{
    public class NotFoundException(string message) : Exception(message);
}