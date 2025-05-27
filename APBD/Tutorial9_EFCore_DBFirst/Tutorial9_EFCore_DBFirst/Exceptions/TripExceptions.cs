namespace Tutorial9_EFCore_DBFirst.Exceptions;

public static class TripExceptions
{
    public class TripNotFoundException(int tripId) :
        BaseExceptions.NotFoundException($"Trip {tripId} not found.");
    public class TripAlreadyOccurredException(int tripId) :
        Exception($"Trip {tripId} already occurred.");
}