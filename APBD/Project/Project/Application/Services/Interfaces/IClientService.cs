using Project.Application.DTOs.Requests;

namespace Project.Application.Services.Interfaces;

public interface IClientService
{
    public Task<int> AddClientAsync(AddClientRequest request, CancellationToken cancellationToken = default);
    public Task DeleteClientAsync(int clientId, CancellationToken cancellationToken = default);
    public Task UpdateClientAsync(int clientId, UpdateClientRequest request, 
        CancellationToken cancellationToken = default);
}