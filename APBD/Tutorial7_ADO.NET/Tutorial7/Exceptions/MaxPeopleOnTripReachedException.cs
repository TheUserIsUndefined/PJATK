namespace Tutorial7.Exceptions;

public class MaxPeopleOnTripReachedException(int tripId) :
    Exception($"The maximum number of people on trip with id={tripId} reached.");