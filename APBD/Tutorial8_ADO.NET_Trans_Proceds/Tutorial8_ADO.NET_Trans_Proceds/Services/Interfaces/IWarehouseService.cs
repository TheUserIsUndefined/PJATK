using Tutorial8_ADO.NET_Trans_Proceds.Contracts.Requests;

namespace Tutorial8_ADO.NET_Trans_Proceds.Services.Interfaces;

public interface IWarehouseService
{
    public Task<int> AddProductToWarehouseAsync(AddProductToWarehouseRequest request,
        CancellationToken token);
    public Task<int> AddProductToWarehouseUsingProcAsync(AddProductToWarehouseRequest request,
        CancellationToken token);
}