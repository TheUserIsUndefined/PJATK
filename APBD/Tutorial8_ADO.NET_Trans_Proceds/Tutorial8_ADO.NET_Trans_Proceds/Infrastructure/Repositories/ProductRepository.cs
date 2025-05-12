using Microsoft.Data.SqlClient;
using Tutorial8_ADO.NET_Trans_Proceds.Infrastructure;
using Tutorial8_ADO.NET_Trans_Proceds.Repositories.Interfaces;

namespace Tutorial8_ADO.NET_Trans_Proceds.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly IUnitOfWork _unitOfWork;

    public ProductRepository(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<double?> GetProductPriceAsync(int productId, CancellationToken token)
    {
        var query = """
                    select Price
                    from Product
                    where IdProduct = @productId
                    """;
        var connection = await _unitOfWork.GetConnectionAsync();
        await using var command = new SqlCommand(query, connection);
        command.Transaction = _unitOfWork.Transaction;
        command.Parameters.AddWithValue("@productId", productId);

        var result = await command.ExecuteScalarAsync(token);
        return result is DBNull or null ? null : Convert.ToDouble(result);
    }
}