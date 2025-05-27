using Tutorial9_EFCore_DBFirst.DAL.Models;
using Tutorial9_EFCore_DBFirst.DTOs.Responses;

namespace Tutorial9_EFCore_DBFirst.Mappers;

public static class TripMapper
{
    public static GetTripDto MapTripToDto(this Trip trip)
    {
        return new GetTripDto
        {
            Name = trip.Name,
            Description = trip.Description,
            DateFrom = trip.DateFrom,
            DateTo = trip.DateTo,
            MaxPeople = trip.MaxPeople,
            Countries = trip.IdCountries.Select(c => c.MapTripToDto()).ToList(),
            Clients = trip.ClientTrips.Select(ct => ct.IdClientNavigation.MapClientToDto()).ToList()
        };
    }
}