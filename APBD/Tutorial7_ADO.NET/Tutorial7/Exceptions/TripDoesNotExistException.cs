namespace Tutorial7.Exceptions;

public class TripDoesNotExistException(int tripId) : Exception($"Trip with id={tripId} does not exist.");