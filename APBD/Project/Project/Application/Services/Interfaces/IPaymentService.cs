using Project.Application.DTOs.Requests;

namespace Project.Application.Services.Interfaces;

public interface IPaymentService
{
    public Task<int> CreatePaymentAsync(int contractId, CreatePaymentRequest request, 
        CancellationToken cancellationToken = default);
}