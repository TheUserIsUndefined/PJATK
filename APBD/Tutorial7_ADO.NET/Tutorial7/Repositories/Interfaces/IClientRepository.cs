using Tutorial7.Contracts.Requests;
using Tutorial7.Contracts.Responses;

namespace Tutorial7.Repositories.Interfaces;

public interface IClientRepository
{
    public Task<IEnumerable<GetAllClientTripsResponse>> GetAllClientTrips(int clientId, 
        CancellationToken cancellationToken);
    public Task<int> CreateClientAsync(CreateClientRequest request, CancellationToken cancellationToken);
    public Task<bool> UpdateClientTripAsync(int clientId, int tripId, CancellationToken cancellationToken);
    public Task<bool> DeleteClientTripAsync(int clientId, int tripId, CancellationToken cancellationToken);
    public Task<bool> DoesClientExistAsync(int clientId, CancellationToken cancellationToken);
    public Task<bool> DoesClientTripExistAsync(int clientId, int tripId, CancellationToken cancellationToken);

}