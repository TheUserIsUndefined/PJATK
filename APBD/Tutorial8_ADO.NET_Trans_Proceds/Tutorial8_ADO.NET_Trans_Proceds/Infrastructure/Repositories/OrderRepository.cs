using Microsoft.Data.SqlClient;
using Tutorial8_ADO.NET_Trans_Proceds.Contracts.DTOs;
using Tutorial8_ADO.NET_Trans_Proceds.Infrastructure;
using Tutorial8_ADO.NET_Trans_Proceds.Repositories.Interfaces;
using Tutorial8_ADO.NET_Trans_Proceds.Services.Interfaces;

namespace Tutorial8_ADO.NET_Trans_Proceds.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IUnitOfWork _unitOfWork;

    public OrderRepository(IDateTimeProvider dateTimeProvider, IUnitOfWork unitOfWork)
    {
        _dateTimeProvider = dateTimeProvider;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<int> GetOrderIdAsync(ProductOrderCheckDto dto, 
        CancellationToken token)
    {
        var query = """
                    select IdOrder
                    from "Order"
                    where IdProduct = @productId
                    and Amount = @amount
                    and CreatedAt < @createdAt
                    """;
        var connection = await _unitOfWork.GetConnectionAsync();
        await using var command = new SqlCommand(query, connection);
        command.Transaction = _unitOfWork.Transaction;
        command.Parameters.AddWithValue("@productId", dto.IdProduct);
        command.Parameters.AddWithValue("@amount", dto.Amount);
        command.Parameters.AddWithValue("@createdAt", dto.CreatedAt);
        
        var result = Convert.ToInt32(await command.ExecuteScalarAsync(token));
        
        return result;
    }

    public async Task UpdateOrderFulfilledAtAsync(int orderId, CancellationToken token)
    {
        var query = """
                    update "Order"
                    set FulfilledAt = @fulfilledAt
                    where IdOrder = @orderId
                    """;
        var connection = await _unitOfWork.GetConnectionAsync();
        await using var command = new SqlCommand(query, connection);
        command.Transaction = _unitOfWork.Transaction;
        command.Parameters.AddWithValue("@fulfilledAt", _dateTimeProvider.Now());
        command.Parameters.AddWithValue("@orderId", orderId);
        
        await command.ExecuteNonQueryAsync(token);
    }
}