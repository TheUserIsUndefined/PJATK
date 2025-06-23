namespace Project.Application.Exceptions;

public static class UserExceptions
{
    public class WrongPasswordException() : Exception("Password is wrong.");
    
    public class UserNotFoundException(string login) : 
        BaseExceptions.NotFoundException($"User {login} not found.");

    public class UserValidationException(string message) : BaseExceptions.ValidationException(message);
}