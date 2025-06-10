using Test_2.Application.DTOs.Responses;

namespace Test_2.Application.Services.Interfaces;

public interface IPublishingHouseService
{
    public Task<ICollection<GetPublishingHousesResponse>> GetPublishingHousesAsync(
        string? city,
        string? country,
        CancellationToken cancellation = default);
}