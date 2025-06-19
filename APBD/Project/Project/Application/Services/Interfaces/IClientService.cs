using Project.Application.DTOs.Requests;

namespace Project.Application.Services.Interfaces;

public interface IClientService
{
    public Task<int> AddClientAsync(AddClientRequest request, CancellationToken cancellationToken = default);
}