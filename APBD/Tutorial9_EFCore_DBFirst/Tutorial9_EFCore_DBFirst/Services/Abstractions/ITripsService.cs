using Tutorial9_EFCore_DBFirst.DTOs.Responses;
using Tutorial9_EFCore_DBFirst.DTOs.Requests;

namespace Tutorial9_EFCore_DBFirst.Services.Abstractions;

public interface ITripsService
{
    public Task<IEnumerable<GetTripDto>> GetAllTripsAsync(CancellationToken cancellationToken);
    public Task<PaginatedResult<GetTripDto>> GetPaginatedTripsAsync(int page, int pageSize, 
        CancellationToken cancellationToken = default);
    public Task<int> AddClientToTripAsync(int idTrip, AddClientToTripRequest request, 
        CancellationToken cancellationToken = default);
}