using Tutorial10_EFCore_CodeFirst.Application.DTOs.Requests;

namespace Tutorial10_EFCore_CodeFirst.Application.Services.Interfaces;

public interface IPrescriptionService
{
    public Task<int> AddPrescriptionAsync(AddPrescriptionRequest request, 
        CancellationToken cancellationToken = default);
}