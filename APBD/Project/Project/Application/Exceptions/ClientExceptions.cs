namespace Project.Application.Exceptions;

public static class ClientExceptions
{
    public class ClientValidationException(string message) : BaseExceptions.ValidationException(message);

    public class ClientNotFoundByIdException(int clientId)
        : BaseExceptions.NotFoundException($"Client with ID {clientId} not found.");
}