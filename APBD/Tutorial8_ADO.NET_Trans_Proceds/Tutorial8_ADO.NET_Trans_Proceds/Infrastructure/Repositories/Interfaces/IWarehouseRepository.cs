namespace Tutorial8_ADO.NET_Trans_Proceds.Repositories.Interfaces;

public interface IWarehouseRepository
{
    public Task<bool> DoesProductExistByIdAsync(int productId, CancellationToken token);
    public Task<bool> DoesWarehouseExistByIdAsync(int warehouseId, CancellationToken token);
    public Task<bool> IsProductHasBeenOrderedAsync(int productId, CancellationToken token);
}