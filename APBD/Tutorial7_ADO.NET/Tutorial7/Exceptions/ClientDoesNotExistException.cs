namespace Tutorial7.Exceptions;

public class ClientDoesNotExistException(int clientId) : Exception($"Client with id={clientId} does not exist.");