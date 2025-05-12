using Tutorial8_ADO.NET_Trans_Proceds.Contracts.DTOs;

namespace Tutorial8_ADO.NET_Trans_Proceds.Repositories.Interfaces;

public interface IOrderRepository
{
    public Task<int> GetOrderIdAsync(ProductOrderCheckDto dto, 
        CancellationToken token);
    public Task UpdateOrderFulfilledAtAsync(int orderId, CancellationToken token);
}