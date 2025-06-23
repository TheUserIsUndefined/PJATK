namespace Project.Application.Exceptions;

public static class RoleExceptions
{
    public class DefaultRoleNotFoundException(string role) : 
        BaseExceptions.NotFoundException($"Default role {role} not found.");
}