using Tutorial7.Contracts.Requests;
using Tutorial7.Contracts.Responses;
using Tutorial7.Exceptions;
using Tutorial7.Repositories.Interfaces;
using Tutorial7.Services.Interfaces;

namespace Tutorial7.Services;

public class ClientService : IClientService
{
    private readonly IClientRepository _clientRepository;
    private readonly ITripRepository _tripRepository;

    public ClientService(IClientRepository clientRepository, ITripRepository tripRepository)
    {
        _clientRepository = clientRepository;
        _tripRepository = tripRepository;
    } 
    
    public async Task<IEnumerable<GetAllClientTripsResponse>> GetAllClientTrips(int clientId, CancellationToken cancellationToken)
    {
        if( !await _clientRepository.DoesClientExistAsync(clientId, cancellationToken) )
            throw new ClientDoesNotExistException(clientId);
        
        var clientTrips = await _clientRepository.GetAllClientTrips(clientId, cancellationToken);
        if (!clientTrips.Any())
            throw new ClientDoesNotHaveTripsException(clientId);
        
        return clientTrips;
    }

    public Task<int> CreateClientAsync(CreateClientRequest request, CancellationToken cancellationToken)
    {
        var clientId = _clientRepository.CreateClientAsync(request, cancellationToken);

        return clientId;
    }

    public async Task<bool> UpdateClientTripAsync(int clientId, int tripId, CancellationToken cancellationToken)
    {
        if( !await _clientRepository.DoesClientExistAsync(clientId, cancellationToken) )
            throw new ClientDoesNotExistException(clientId);
        if( !await _tripRepository.DoesTripExistAsync(tripId, cancellationToken))
            throw new TripDoesNotExistException(tripId);
        if (await _tripRepository.IsMaxPeopleOnTripReachedAsync(tripId, cancellationToken))
            throw new MaxPeopleOnTripReachedException(tripId);
        if (await _clientRepository.DoesClientTripExistAsync(clientId, tripId, cancellationToken))
            throw new ClientTripExistsException(clientId, tripId);
        
        var result = await _clientRepository.UpdateClientTripAsync(clientId, tripId, cancellationToken);

        return result;
    }

    public async Task<bool> DeleteClientTripAsync(int clientId, int tripId, CancellationToken cancellationToken)
    {
        if ( !await _clientRepository.DoesClientTripExistAsync(clientId, tripId, cancellationToken) )
            throw new ClientTripDoesNotExistException(clientId, tripId);
        
        var result = await _clientRepository.DeleteClientTripAsync(clientId, tripId, cancellationToken);
        
        return result;
    }
}