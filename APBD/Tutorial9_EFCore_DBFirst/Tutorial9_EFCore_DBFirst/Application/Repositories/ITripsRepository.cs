using Tutorial9_EFCore_DBFirst.DAL.Models;
using Tutorial9_EFCore_DBFirst.DTOs.Responses;

namespace Tutorial9_EFCore_DBFirst.DAL.Infrastructure.Repositories.Abstractions;

public interface ITripsRepository
{
    public Task<IEnumerable<Trip>> GetAllTripsAsync(CancellationToken cancellationToken = default); 
    public Task<PaginatedResult<Trip>> GetPaginatedTripsAsync(int page, int pageSize, 
        CancellationToken cancellationToken = default);
    public Task<bool> IsClientRegisteredOnTripAsync(int clientId, int tripId,
        CancellationToken cancellationToken = default);
    public Task<bool> TripExistsByIdAsync(int tripId, CancellationToken cancellationToken = default);
    public Task<bool> HasTripAlreadyOccurredAsync(int tripId, CancellationToken cancellationToken = default);
    public Task<bool> AddClientTripAsync(ClientTrip clientTrip, CancellationToken cancellationToken = default);
}