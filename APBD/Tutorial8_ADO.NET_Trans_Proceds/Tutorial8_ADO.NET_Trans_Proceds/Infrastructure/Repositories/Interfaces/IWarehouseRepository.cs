using Tutorial8_ADO.NET_Trans_Proceds.Contracts.DTOs;
using Tutorial8_ADO.NET_Trans_Proceds.Contracts.Requests;

namespace Tutorial8_ADO.NET_Trans_Proceds.Repositories.Interfaces;

public interface IWarehouseRepository
{
    public Task<bool> DoesWarehouseExistByIdAsync(int warehouseId, CancellationToken token);
    public Task<bool> HasOrderBeenCompletedAsync(int orderId, CancellationToken token);
    public Task<int> InsertWarehouseProductAsync(InsertWarehouseProductDto dto, 
        CancellationToken token);
    public Task<int> AddProductToWarehouseUsingProcAsync(AddProductToWarehouseRequest request, 
        CancellationToken token);
}