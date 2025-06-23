using Project.Core.Models;

namespace Project.Application.Services.Interfaces;

public interface IRevenueService
{
    public Task<string> GetCurrentRevenueAsync(int? softwareId = null, string? currency = null,
        CancellationToken cancellationToken = default);
    
    public Task<string> GetPredictedRevenueAsync(int? softwareId = null, string? currency = null,
        CancellationToken cancellationToken = default);

    public Task<string> GetRevenueAsync(ContractStatus[] filterBy, int? softwareId = null, string? currency = null,
        CancellationToken cancellationToken = default);
}