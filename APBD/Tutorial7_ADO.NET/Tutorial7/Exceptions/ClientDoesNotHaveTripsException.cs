namespace Tutorial7.Exceptions;

public class ClientDoesNotHaveTripsException(int clientId) : Exception($"Client with id={clientId} does not have any trips.");