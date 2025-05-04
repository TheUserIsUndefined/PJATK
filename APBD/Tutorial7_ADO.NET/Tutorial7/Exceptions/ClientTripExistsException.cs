namespace Tutorial7.Exceptions;

public class ClientTripExistsException (int clientId, int tripId) :
    Exception($"Client {clientId} already registered on trip {tripId}.");