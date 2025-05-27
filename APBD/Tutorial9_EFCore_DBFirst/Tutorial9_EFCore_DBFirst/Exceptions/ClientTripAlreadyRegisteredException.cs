namespace Tutorial9_EFCore_DBFirst.Exceptions;

public class ClientTripAlreadyRegisteredException(int clientId, int tripId) : 
    Exception($"Client {clientId} already registered on trip {tripId}.");