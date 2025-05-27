namespace Tutorial9_EFCore_DBFirst.Exceptions;

public static class ClientExceptions
{
    public class ClientHasTripsException(int clientId) : InvalidOperationException($"Client {clientId} has trips.");
    public class ClientNotFoundException(int clientId) : 
        BaseExceptions.NotFoundException($"Client {clientId} not found.");
}