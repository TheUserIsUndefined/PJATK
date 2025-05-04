using Tutorial7.Contracts.Responses;

namespace Tutorial7.Repositories.Interfaces;

public interface ITripRepository
{ 
    Task<IEnumerable<GetAllTripsResponse>> GetAllTripsAsync(CancellationToken cancellationToken);
    public Task<bool> DoesTripExistAsync(int tripId, CancellationToken cancellationToken);
    public Task<bool> IsMaxPeopleOnTripReachedAsync(int tridId, CancellationToken cancellationToken);
}