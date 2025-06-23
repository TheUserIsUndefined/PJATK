namespace Project.Application.Exceptions;

public static class SoftwareExceptions
{
    public class SoftwareValidationException(string message) : BaseExceptions.ValidationException(message);
    public class SoftwareNotFoundByIdException(int softwareId) : 
        BaseExceptions.NotFoundException($"Software with ID {softwareId} not found.");
}