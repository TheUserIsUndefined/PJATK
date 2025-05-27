using Tutorial9_EFCore_DBFirst.DAL.Repositories.Abstractions;
using Tutorial9_EFCore_DBFirst.Exceptions;
using Tutorial9_EFCore_DBFirst.Services.Abstractions;

namespace Tutorial9_EFCore_DBFirst.Services;

public class ClientsService : IClientsService
{
    private readonly IClientsRepository _clientsRepository;

    public ClientsService(IClientsRepository clientsRepository)
    {
        _clientsRepository = clientsRepository;
    }
    
    public async Task<bool> DeleteClientAsync(int idClient, CancellationToken cancellationToken = default)
    {
        if (idClient < 1)
            throw new ArgumentException("Client id should be greater than 0.");
        
        var clientHasTrips = await _clientsRepository.ClientHasTripsAsync(idClient, cancellationToken);
        if (clientHasTrips)
            throw new ClientExceptions.ClientHasTripsException(idClient);
        
        var clientDeleted = await _clientsRepository.DeleteClientAsync(idClient, cancellationToken);
        if (!clientDeleted)
            throw new ClientExceptions.ClientNotFoundException(idClient);
        
        return clientDeleted;
    }
}