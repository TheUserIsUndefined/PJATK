using Tutorial8_ADO.NET_Trans_Proceds.Infrastructure;
using Tutorial8_ADO.NET_Trans_Proceds.Repositories.Interfaces;
using Tutorial8_ADO.NET_Trans_Proceds.Services;

namespace Tutorial8_ADO.NET_Trans_Proceds.Repositories;

public class WarehouseRepository : IWarehouseRepository
{
    private readonly IUnitOfWork _uow;

    public WarehouseService(IUnitOfWork uow)
    {
        _uow = uow;
    }
    
    public Task<bool> DoesProductExistByIdAsync(int productId, CancellationToken token)
    {
        var query = """
                    select 1
                    from Product
                    where IdProduct = @productId
                    """;
        
        
    }

    public Task<bool> DoesWarehouseExistByIdAsync(int warehouseId, CancellationToken token)
    {
        var query = """
                    select 1
                    from Warehouse
                    where IdWarehouse = @warehouseId
                    """;
        
        
    }

    public Task<bool> IsProductHasBeenOrderedAsync(int productId, int productAmount, DateTime createdAt, 
        CancellationToken token)
    {
        var query = """
                    select 1
                    from Order
                    where IdProduct = @productId
                    and Amount >= @productAmount
                    and CreatedAt < @createdAt
                    """;
    }
}