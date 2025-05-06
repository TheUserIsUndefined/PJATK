using Tutorial8_ADO.NET_Trans_Proceds.Contracts.Requests;
using Tutorial8_ADO.NET_Trans_Proceds.Repositories.Interfaces;
using Tutorial8_ADO.NET_Trans_Proceds.Services.Interfaces;

namespace Tutorial8_ADO.NET_Trans_Proceds.Services;

public class WarehouseService : IWarehouseService
{
    private readonly IWarehouseRepository _warehouseRepository;

    public WarehouseService(IWarehouseRepository warehouseRepository)
    {
        _warehouseRepository = warehouseRepository;
    }
    
    public async Task<int> AddProductToWarehouseAsync(AddProductToWarehouseRequest request, CancellationToken token)
    {
        
    }
}