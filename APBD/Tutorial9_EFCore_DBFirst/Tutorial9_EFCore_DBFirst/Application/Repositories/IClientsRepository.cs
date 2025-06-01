using Tutorial9_EFCore_DBFirst.DAL.Models;

namespace Tutorial9_EFCore_DBFirst.DAL.Infrastructure.Repositories.Abstractions;

public interface IClientsRepository
{
    public Task<bool> ClientHasTripsAsync(int clientId, CancellationToken cancellationToken = default);
    public Task<int> GetClientIdByPeselAsync(string pesel, CancellationToken cancellationToken = default);
    public Task<int> AddClientAsync(Client client, CancellationToken cancellationToken = default);
    public Task<bool> DeleteClientAsync(int clientId, CancellationToken cancellationToken = default);
}