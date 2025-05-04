using Tutorial7.Contracts.Responses;

namespace Tutorial7.Services.Interfaces;

public interface ITripService
{ 
    public Task<IEnumerable<GetAllTripsResponse>> GetAllTripsAsync(CancellationToken cancellationToken);
}