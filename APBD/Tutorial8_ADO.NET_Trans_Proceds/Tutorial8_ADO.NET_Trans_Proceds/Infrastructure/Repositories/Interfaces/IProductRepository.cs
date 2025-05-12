namespace Tutorial8_ADO.NET_Trans_Proceds.Repositories.Interfaces;

public interface IProductRepository
{
    public Task<double?> GetProductPriceAsync(int productId, CancellationToken token);
}