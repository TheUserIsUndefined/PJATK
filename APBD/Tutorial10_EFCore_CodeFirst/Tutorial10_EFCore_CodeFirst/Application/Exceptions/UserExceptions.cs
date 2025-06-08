namespace Tutorial10_EFCore_CodeFirst.Application.Exceptions;

public static class UserExceptions
{
    public class WrongPasswordException() : Exception("Password is wrong.");
    
    public class UserNotFoundException(string login) : 
        BaseExceptions.NotFoundException($"User {login} not found.");
}