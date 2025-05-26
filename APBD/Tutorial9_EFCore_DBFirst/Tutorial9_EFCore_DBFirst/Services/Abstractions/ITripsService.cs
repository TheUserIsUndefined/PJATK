using Tutorial9_EFCore_DBFirst.DTOs;

namespace Tutorial9_EFCore_DBFirst.Services.Abstractions;

public interface ITripsService
{
    public Task<IEnumerable<GetTripDto>> GetAllTripsAsync(CancellationToken cancellationToken);
}