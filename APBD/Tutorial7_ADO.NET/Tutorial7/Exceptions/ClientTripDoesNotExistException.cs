namespace Tutorial7.Exceptions;

public class ClientTripDoesNotExistException(int clientId, int tripId) :
    Exception($"Client {clientId} does not registered on trip {tripId}.");