using Project.Application.DTOs.Requests;

namespace Project.Application.Services.Interfaces;

public interface IContractService
{
    public Task<int> CreateContractAsync(
        CreateContractRequest request,
        CancellationToken cancellationToken = default);
}